using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;

namespace Cel.GameOfLife.API.Conventions;

public class MultipleProducesResponseTypeConvention : IActionModelConvention
{
    private readonly (int statusCode, Type type)[] _defaults;

    public MultipleProducesResponseTypeConvention(params (int, Type)[] defaults)
    {
        _defaults = defaults;
    }

    public void Apply(ActionModel action)
    {
        var existingAttributes = action.Filters.OfType<ProducesResponseTypeAttribute>().Select(a => a.StatusCode).ToHashSet();

        foreach (var (statusCode, type) in _defaults)
        {
            if (!existingAttributes.Contains(statusCode))
            {
                action.Filters.Add(new ProducesResponseTypeAttribute(type, statusCode));
            }
        }
    }
}