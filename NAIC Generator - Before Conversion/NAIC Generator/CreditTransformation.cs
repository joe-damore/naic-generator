namespace naic
{
    /**
    \brief
        Describes a credit transformation

        Credit transformations describe
        a number of credits being
        moved or copied from one
        line of authority to another
    */
    public class CreditTransformation
    {
        /// The type of credits to be
        /// transformed
        public CreditType SourceType { get; set; }

        /// The type of credits for transformation
        /// to output to
        public CreditType DestinationType { get; set; }

        /// Action for credit transformation
        public TransformationAction Action { get; set; }

        /// Number of credits to be transformed
        public TransformationAmount Amount { get; set; }

        /// How credits are applied to destination
        /// line of authority
        public TransformationDestination Destination { get; set; }
    }
}
