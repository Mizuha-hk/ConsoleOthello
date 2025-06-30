using OthelloApp.Core;
using Microsoft.Extensions.DependencyInjection;

namespace OthelloApp.ConsoleApp;

public static class OthelloAppExtensions
{
    public static OthelloAppBuilder AddView<T>(this OthelloAppBuilder builder)
        where T : class
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder), "OthelloAppBuilderはnullであってはいけません。");
        if (typeof(T).IsClass == false) throw new ArgumentException("Tはクラスでなければなりません。", nameof(T));

        builder.Services.AddTransient<T>();
        return builder;
    }

    public static OthelloAppBuilder AddController<T>(this OthelloAppBuilder builder)
        where T : class
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder), "OthelloAppBuilderはnullであってはいけません。");
        if (typeof(T).IsClass == false) throw new ArgumentException("Tはクラスでなければなりません。", nameof(T));

        builder.Services.AddScoped<T>();
        return builder;
    }

    public static OthelloAppBuilder AddSubject<T>(this OthelloAppBuilder builder)
        where T : class
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder), "OthelloAppBuilderはnullであってはいけません。");
        if (typeof(T).IsClass == false) throw new ArgumentException("Tはクラスでなければなりません。", nameof(T));
        builder.Services.AddScoped<T>();
        return builder;
    }

    public static OthelloAppBuilder AddPresenter<I, T>(this OthelloAppBuilder builder)
        where I : class
        where T : class, I
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder), "OthelloAppBuilderはnullであってはいけません。");
        if (typeof(I).IsInterface == false) throw new ArgumentException("Iはインターフェースでなければなりません。", nameof(I));
        if (typeof(T).IsClass == false) throw new ArgumentException("Tはクラスでなければなりません。", nameof(T));
        builder.Services.AddScoped<I, T>();
        return builder;
    }
}
