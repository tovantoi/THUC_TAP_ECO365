namespace _365EJSC.ERP.Contract.Shared
{
    public class SkipTakeQuery
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public SkipTakeQuery()
        {
        }

        public SkipTakeQuery(int? skip, int? take)
        {
            Skip = skip < 0 ? null : skip;
            Take = take < 0 ? null : take;
        }
    }
}