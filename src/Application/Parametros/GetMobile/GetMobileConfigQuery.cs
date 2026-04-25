using Application.Abstractions.Messaging;

namespace Application.Parametros.GetMobile;

public sealed record GetMobileConfigQuery() : IQuery<MobileConfigResponse>;