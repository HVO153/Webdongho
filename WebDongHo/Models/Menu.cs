using System;
using System.Collections.Generic;

namespace WebDongHo.Models;

public partial class Menu
{
    public int IdMenu { get; set; }

    public string? Title { get; set; }

    public int? Order { get; set; }

    public string? Link { get; set; }

    public int Hide { get; set; }
}
