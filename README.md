# 辅助工具说明

## 简介

该工具集主要是我将过去应用开发中常用的一些方法提取出来组合而成的。我的每一款应用几乎都会用到其中的大部分方法，每次都重复书写显然不太好，所以取出来以降低工作量

## 使用

### 配置项

工具使用`Options`类作为初始化的配置项进行注入，配置项内包含一些简单的自定义选项：

- SettingContainerName : 创建本地设置容器
- DefaultNotificationLogoPath : 推送通知时默认的图标路径
- DarkButtonHoverColor 等颜色 : 相关按钮颜色用于设置TitleBar中的操作按钮颜色
- LocalizationStringPrefix : 在定义本地化文本资源时，我通常会用给资源名加前缀用以区分哪些是给控件用的，哪些是给普通文本用的，该选项用于设置默认的读取本地化文本资源的前缀（或者说限定）

```csharp
var option = new Options();
option.DarkButtonHoverColor = Colors.DarkGray;
option.DarkButtonPressColor = Colors.Black;
option.LightButtonHoverColor = Colors.White;
option.LightButtonPressColor = Colors.Wheat;
option.LocalizationStringPrefix = "Tip_";
option.SettingContainerName = "SampleApp";
```

### 创建工具类实例

考虑到实际使用中的情况，这里需要创建一个实例来调用各个帮助模块内的方法：

```csharp
_instance = new Instance(option);
```

这里建议将Instance作为一个公开的静态变量写在`App.xaml.cs`中，这样可以在后续通过`App._instance`进行调用。

Instance的初始化必须要在应用加载后进行（比如进入`OnLaunched`生命周期函数）。在Instance构建过程中，会对当前窗口添加一个SizeChanged的事件监听，如果应用窗口还未加载，则此事件在后续应用运行的过程中不会触发。

### 窗口方法

UWP应用在桌面环境下宽度不一，在一些需要根据窗口大小调整内容的使用场景中，需要监听窗口大小的变化。

在工具类中，可以通过调用`AddWindowSizeChangeAction`方法，来添加对应的事件处理方法：

```csharp
_guid = Guid.NewGuid();
_instance.AddWindowSizeChangeAction(_guid, (size) =>
{
    Width = size.Width;
    Height = size.Height;
    // Do other things...
});
```

事件处理方法应当和传入的Guid一一对应，在不需要监听窗口变化时，及时移除对应方法：

```csharp
_instance.RemoveWindowSizeChangeAction(_guid);
```

### 应用模块

#### 本地设置

关于本地设置，提供了三种方法：`WriteLocalSetting`，`GetLocalSetting`，`GetBoolSetting`。

我通常会避免往`LocalSettings`中放入非字符串的类型，主要原因是避免进行可能出现的类型转换错误。避免犯错的一个好方法就是只给一个选择，所以在读取/写入本地设置时，要求使用者提供枚举作为Key。

比如：

```csharp
public enum Settings
{
    Theme,
    Version
}

string currentTheme = _instance.App.GetLocalSetting(Settings.Theme, "Light");
```

*使用枚举的好处就是不至于写错设置名，还能有智能提示。*

在使用GET方法时，会要求提供一个默认值，该值会在设置项未创建时返回，同时在本地创建设置项并赋值。

`GetBoolSetting`方法是对`GetLocalSetting`的包装，将输出结果转化为布尔值，在某些情况下，使用频率还是很高的。

#### 日期-时间戳

辅助工具类提供了一些简单的方法用于获取当前的Unix时间戳（秒级），以及在DateTime和Unix时间戳之间进行转换。

**事实上，`DateTimeOffset`类也可以做相同的事。**

```csharp
int timeSeconds = _instance.App.GetNowSeconds();
int timeMilliSeconds = _instance.App.GetNowTimeStamp();
int customTimeSeconds = _instance.App.DateToTimeStamp(DateTime.Now);

DateTime date = _instance.App.TimeStampToDate(timeSeconds);
```

#### 标题栏

UWP的标题栏往往是做应用设计时最先干掉的东西，大多数情况下，它都很碍事。

帮助工具提供了`SetTitleBarColor`方法用于快速去除标题栏，并将`Options`类中的相关颜色定义应用于右上角的按钮上。

```csharp
_instance.App.SetTitleBarColor(currentTheme);
```

#### 资源读取

我通常会在应用中定义很多的资源文件，包括国际化文本、字体、控件样式等，某些时候需要在代码中动态切换。通常我们可以用`Application.Current.Resources`来读取当前应用的资源，不过谁介意再多包一层，来省去我们进行类型转化的麻烦呢？

```csharp
public enum Fonts
{
    Icon,
    BasicFont
}

public enum LanguageType
{
    Hello,
    Next
}

FontFamily textFont = _instance.App.GetFontFamilyFromResource(Fonts.BasicFont);

string hello = _instance.App.GetLocalizationTextFromResource(LanguageType.Hello); // get the Tip_Hello resource text.
string next = _instance.App.GetLocalizationTextFromResource("Tip_Next");
```

### IO模块

这里的IO主要指的是文件的读写。

#### 打开及保存文件

在UWP中推荐的访问文件的方式是通过文件选择器，而不是通过路径访问（权限问题），工具类对此进行了封装：

```csharp
var file = await _instance.IO.OpenLocalFileAsync(".png", ".jpg");

var needSaveFile = await _instance.IO.GetSaveFileAsync(".png", "测试图片.png", "PNG图片文件");
```

#### 存取本地数据

在应用运行过程中往往需要保存一些应用数据，比如用户列表或者历史记录。这些数据往往是结构化的，而且数量可能比较多，存在LocalSettings里面就不太合适了。我们往往需要将其保存在应用文件夹中，我通常使用JSON将这些数据序列化并存在本地。

```csharp
// write
string myData = JsonConvert.SerializeObject(myList);
await _instance.IO.SetLocalDataAsync("MyList.json", myData);

// read
var myList = await _instance.IO.GetLocalDataAsync<List<MyItem>>("MyList.json");
```

### 应用通知

通知模块乏善可陈，基本是对一些发送通知的常见操作进行了封装。这里引用了`Microsoft.Toolkit.Uwp.Notifications`的内容，以帮助我通过C#代码创建ToastContent，而不是通过XML。

通知提供了一个`NotificationItem`类用于定义非操作通知（即不包含按钮等可操作部件），可以通过模块中提供的`ShowToast`方法进行批量发送。

`ShowToast`(列表推送)方法提供了一个通知最大条目的限制，如果您需要这个限制，则需要额外提供`overflowText`，该文本用于提醒用户存在未显示的通知。

```csharp
var noticationList = new List<NotificationItem>();
// ... data init
_instance.Notification.ShowToast(noticaitionList, "更多更新请打开应用查看", 3, "dynamic");
```

### 网络请求

网络请求模块封装了`Windows.Web.Http.HttpClient`的一些常用操作：

- GetTextFromWebAsync : 用于从网络获取文本，通常是请求API，回传JSON文本
- GetEntityFromWebAsync<T> : 从网络获取数据，并将数据转化为指定类型的实例。这里限制网络数据必须为JSON字符串
- PostContentToWebAsync : 向网络上传文本内容。

三个方法都可以传入自定义的请求头。同时由于使用了`HttpBaseProtocolFilter`，默认是带Cookie的，这可以省去某些网站做Cookie验证的麻烦。

```csharp
var header = new Dictionary<string,string>();
header.Add("user-agent","xxxxx");
List<WebItem> WebData = await _instance.Web.GetEntityFromWebAsync<List<WebItem>>(myUrl, header);
```

### MD5

这个模块比较特殊，不适用于常见情况，是针对某些特殊场景写的MD5转化。

.NET 提供了MD5加密的方法，这里不赘述了。

该模块使用很简单：

```csharp
string md5 = _instance.MD5.GetMd5String("Hello World");
```

---

东西不多，一些并不通用的方法就不放上来了。