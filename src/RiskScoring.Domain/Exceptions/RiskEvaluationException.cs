namespace RiskScoring.Domain.Exceptions;

public class RiskEvaluationException : Exception
{
    public RiskEvaluationException(string message) : base(message) { }
    public RiskEvaluationException(string message, Exception inner) : base(message, inner) { }

}