namespace HaloStats.Web.Client.Infrastructure.Extensions;

public static class SortDirectionMapper
{
    public static Shared.Infrastructure.SortDirection ToSortDirection(this MudBlazor.SortDirection msd)
    {
        switch (msd)
        {
            case MudBlazor.SortDirection.None:
                return Shared.Infrastructure.SortDirection.None;
            case MudBlazor.SortDirection.Ascending:
                return Shared.Infrastructure.SortDirection.Ascending;
            case MudBlazor.SortDirection.Descending:
                return Shared.Infrastructure.SortDirection.Descending;
            default:
                return Shared.Infrastructure.SortDirection.None;
        }
    }
}
