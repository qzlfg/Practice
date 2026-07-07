using System;
using System.Text.Json.Serialization;


[JsonSerializable]
class Subject
{
    public string Name {get; set; }
    public int Grade {get; set; }
}