using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Model;

public record PaginationRequest(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(10)]
    [property: FromQuery(Name = "pageSize")]
    int PageSize = 10,

    [property: Description("The index of the page of results to return")]
    [property: DefaultValue(0)]
    [property: FromQuery(Name = "pageIndex")]
    int PageIndex = 0
);