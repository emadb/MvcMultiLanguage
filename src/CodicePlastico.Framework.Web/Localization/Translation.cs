namespace CodicePlastico.Framework.Web.Localization
{
    public class Translation 
    {
        public virtual int Id { get; set; }
        public virtual string Key { get; set; }
        public virtual string Language { get; set; }
        public virtual string Value { get; set; }
    }
}