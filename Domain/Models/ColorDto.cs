﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ColorDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
    }
}
