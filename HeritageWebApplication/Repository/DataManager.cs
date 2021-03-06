﻿using HeritageWebApplication.Models;

namespace HeritageWebApplication.Repository
{
    public class DataManager : IDataManager
    {
        public IRepository<Building> BuildingRepository { get; set; }
        public IRepository<HeritageObject> HeritageObjectRepository { get; set; } 
        public IRepository<Comment> CommentRepository { get; set; }

        public IRepository<RenovationCompany> RenovatoinCompanyRepository { get; set; }

        public DataManager(IRepository<Building> buildingRepository, 
            IRepository<HeritageObject> heritageObjectRepository,
            IRepository<Comment> commentRepository,
            IRepository<RenovationCompany> renovatoinCompanyRepository)
        {
            this.BuildingRepository = buildingRepository;
            this.HeritageObjectRepository = heritageObjectRepository;
            this.CommentRepository = commentRepository;
            this.RenovatoinCompanyRepository = renovatoinCompanyRepository;
        }
    }
}