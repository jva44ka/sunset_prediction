using Domain.Entities.Enums;

namespace Domain.Extensions;

public static class SubscribeTypeExtension
{
    public static SubscribeType Union(
        this SubscribeType sourceSubscribeType,
        SubscribeType secondSubscribeType)
    {
        return secondSubscribeType | secondSubscribeType;
    }

    public static SubscribeType Subtract(
        this SubscribeType sourceSubscribeType,
        SubscribeType secondSubscribeType)
    {
        return sourceSubscribeType & (secondSubscribeType ^ (SubscribeType)byte.MaxValue);
    }
}