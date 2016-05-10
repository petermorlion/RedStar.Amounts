namespace RedStar.Amounts
{
    /// <summary>
    /// This interface represents a consumer of a unit, such as an Amount.
    /// </summary>
    public interface IUnitConsumer
    {
        /// <summary>
        /// The unit of the consumer.
        /// </summary>
        Unit Unit { get; }
    }
}