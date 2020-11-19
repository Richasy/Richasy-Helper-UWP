namespace Richasy.Helper.UWP.Models
{
    public class Translate
    {
        public int status { get; set; }
        public Content content { get; set; }
    }

    public class Content
    {
        public string from { get; set; }
        public string to { get; set; }
        public string @out { get; set; }
        public string vendor { get; set; }
        public int err_no { get; set; }
    }

    public class Ciba
    {
        public string word_name { get; set; }
        public string is_CRI { get; set; }
        public Exchange exchange { get; set; }
        public Symbol[] symbols { get; set; }
    }

    public class Exchange
    {
        public string[] word_pl { get; set; }
        public string[] word_third { get; set; }
        public string[] word_past { get; set; }
        public string[] word_done { get; set; }
        public string[] word_ing { get; set; }
        public string[] word_er { get; set; }
        public string[] word_est { get; set; }
    }

    public class Symbol
    {
        public string ph_en { get; set; }
        public string ph_am { get; set; }
        public string ph_other { get; set; }
        public string ph_en_mp3 { get; set; }
        public string ph_am_mp3 { get; set; }
        public string ph_tts_mp3 { get; set; }
        public string word_symbol { get; set; }
        public string symbol_mp3 { get; set; }
        public Part[] parts { get; set; }
    }

    public class Part
    {
        public string part { get; set; }
        public string[] means { get; set; }
        public Mean[] means_other { get; set; }
    }

    public class Mean
    {
        public string word_mean { get; set; }
        public string has_mean { get; set; }
        public int split { get; set; }
    }
}
