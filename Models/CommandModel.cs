
namespace sp_macro
{
    public class CommandModel
    {
        public string Code { get; set; }
        public string BinaryCode { get; set; }
        public int Length { get; set; }
        public bool EqualsMnemonic(string mnemonic)
        {
            return this.Code == mnemonic;
        }
        public override bool Equals(object obj)
        {
            CommandModel i = obj as CommandModel;
            return this.Code == i.Code;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
