namespace SharedKernel;

public sealed record ItemResponse<T>(
    T Value,
    string Label);