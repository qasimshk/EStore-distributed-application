namespace orchestrator.service.Extensions;

using MassTransit;

public class CustomEndpointNameFormatter : IEndpointNameFormatter
{
    private readonly IEndpointNameFormatter _formatter;

    public string Separator => throw new NotImplementedException();

    public CustomEndpointNameFormatter() =>
        _formatter = KebabCaseEndpointNameFormatter.Instance;

    public string TemporaryEndpoint(string tag) => _formatter.TemporaryEndpoint(tag);

    public string Consumer<T>() where T : class, IConsumer => _formatter.Consumer<T>();

    public string Message<T>() where T : class => _formatter.Message<T>();

    public string Saga<T>() where T : class, ISaga => _formatter.Saga<T>();

    public string ExecuteActivity<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class
    {
        var executeActivity = _formatter.ExecuteActivity<T, TArguments>();

        return SanitizeName(executeActivity);
    }

    public string CompensateActivity<T, TLog>()
        where T : class, ICompensateActivity<TLog>
        where TLog : class
    {
        var compensateActivity = _formatter.CompensateActivity<T, TLog>();

        return SanitizeName(compensateActivity);
    }

    public string SanitizeName(string name) => _formatter.SanitizeName(name);
}
