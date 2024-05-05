using Blazorise.Charts;

namespace Aaru.Server.New.Components.Pages.Statistics;

public static class Common
{
    internal static readonly List<string> _backgroundColors =
    [
        ChartColor.FromHtmlColorCode("#006412"), ChartColor.FromHtmlColorCode("#0000D3"),
        ChartColor.FromHtmlColorCode("#FF6403"), ChartColor.FromHtmlColorCode("#562C05"),
        ChartColor.FromHtmlColorCode("#DD0907"), ChartColor.FromHtmlColorCode("#F20884"),
        ChartColor.FromHtmlColorCode("#4700A5"), ChartColor.FromHtmlColorCode("#90713A"),
        ChartColor.FromHtmlColorCode("#1FB714"), ChartColor.FromHtmlColorCode("#02ABEA"),
        ChartColor.FromHtmlColorCode("#FBF305")
    ];
    internal static readonly List<string> _borderColors = [ChartColor.FromHtmlColorCode("#8b0000")];

    internal static async Task HandleRedraw<TDataSet, TItem, TOptions, TModel>(
        BaseChart<TDataSet, TItem, TOptions, TModel> chart, string[] labels, Func<TDataSet> getDataSet)
        where TDataSet : ChartDataset<TItem> where TOptions : ChartOptions where TModel : ChartModel
    {
        await chart.Clear();

        await chart.AddLabelsDatasetsAndUpdate(labels, getDataSet());
    }
}