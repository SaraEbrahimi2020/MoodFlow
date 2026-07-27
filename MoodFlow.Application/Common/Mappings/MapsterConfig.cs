using Mapster;
using Moodflow.Domain.Entities;
using MoodFlow.Application.Features.MoodLogs.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoodFlow.Application.Common.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        // نگاشت دستی برای مواردی که فیلد ویژه دارند (در صورت نیاز)
        TypeAdapterConfig<MoodLog, MoodLogResponse>
            .NewConfig();
        // .Map(dest => dest.SomeProperty, src => src.SomeOtherProperty) // اگر اسمی فرق داشت
    }
}
