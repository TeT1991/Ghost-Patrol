namespace NuclearDecline
{
    public enum AdResultStatus
    {
        Success,
        Failed,
        Closed,
        Skipped
    }

    public readonly struct AdResult
    {
        public static readonly AdResult Completed = SuccessResult();
        public static readonly AdResult Skipped = SkippedResult();
        public static readonly AdResult Failed = FailedResult();
        public static readonly AdResult NotAvailable = FailedResult(null, "Ad is not available.");

        public AdResultStatus Status { get; }
        public string PlacementId { get; }
        public string ErrorMessage { get; }

        public bool IsSuccess => Status == AdResultStatus.Success;
        public bool IsFailed => Status == AdResultStatus.Failed;
        public bool IsClosed => Status == AdResultStatus.Closed;
        public bool IsSkipped => Status == AdResultStatus.Skipped;

        private AdResult(AdResultStatus status, string placementId, string errorMessage)
        {
            Status = status;
            PlacementId = placementId;
            ErrorMessage = errorMessage;
        }

        public static AdResult SuccessResult(string placementId = null)
        {
            return new AdResult(AdResultStatus.Success, placementId, null);
        }

        public static AdResult FailedResult(string placementId = null, string errorMessage = null)
        {
            return new AdResult(AdResultStatus.Failed, placementId, errorMessage);
        }

        public static AdResult ClosedResult(string placementId = null)
        {
            return new AdResult(AdResultStatus.Closed, placementId, null);
        }

        public static AdResult SkippedResult(string placementId = null)
        {
            return new AdResult(AdResultStatus.Skipped, placementId, null);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(PlacementId) && string.IsNullOrEmpty(ErrorMessage))
                return Status.ToString();

            if (string.IsNullOrEmpty(ErrorMessage))
                return Status + " (" + PlacementId + ")";

            return Status + " (" + PlacementId + "): " + ErrorMessage;
        }
    }
}
