﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Taller.Aplication.Mc.Dtos.MiningConcessions
{
    public class MiningConcessionDto
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int MineralTypeId { get; set; }

        public int OriginId { get; set; }

        public int TypeId { get; set; }

        public int SituationId { get; set; }

        public int MiningUnitId { get; set; }

        public int ConditionId { get; set; }

        public int StateManagementId { get; set; }
        public bool State { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }

    }
}
