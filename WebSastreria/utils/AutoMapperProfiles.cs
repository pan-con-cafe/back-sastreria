using AutoMapper;
using sastreria_data.database.tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.entities;
using sastreria_domain.RequestResponse;
using WebSastreria.models;

namespace WebSastreria.utils

{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {

            //categoria
            CreateMap<Categoria, CategoriaResponse>().ReverseMap();
            CreateMap<Categoria, CategoriaRequest>().ReverseMap();
            CreateMap<CategoriaResponse, CategoriaRequest>().ReverseMap();

            // Modelo
            CreateMap<ModeloDomain, ModeloResponse>().ReverseMap();
            CreateMap<ModeloRequest, ModeloDomain>().ReverseMap();

            CreateMap<Modelo, ModeloDomain>().ReverseMap();
        } 
       

    }
}
