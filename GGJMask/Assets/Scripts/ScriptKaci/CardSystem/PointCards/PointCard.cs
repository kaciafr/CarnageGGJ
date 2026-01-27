namespace ScriptKaci
{
    public class PointCard : ICard
    {
        public readonly PointCardData data;

        public PointCard(PointCardData data)
        {
            this.data = data;
        }
    }
}