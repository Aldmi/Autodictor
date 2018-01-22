using System;
using System.ServiceModel.PeerResolvers;
using AutoMapper;
using Communication.SibWayApi;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Mappers
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
                cfg.CreateMap<UniversalInputType, ItemSibWay>()
                    .ForMember(dest => dest.TimeArrival,
                        opt => opt.MapFrom(src => (src.TransitTime != null && src.TransitTime.ContainsKey("приб")) ? src.TransitTime["приб"] : DateTime.MinValue))
                    .ForMember(dest => dest.TimeDeparture,
                        opt => opt.MapFrom(src => (src.TransitTime != null && src.TransitTime.ContainsKey("отпр")) ? src.TransitTime["отпр"] : DateTime.MinValue))
                    .ForMember(dest => dest.PathNumber,
                        opt => opt.MapFrom(src => src.PathNumberWithoutAutoReset)));
        }
    }
}