using System;

public class Reading
{
    public int Id { get; set; }
    public DateTime Time { get; set; }       // Дата на Submit
    public double Power { get; set; }        // AC Мощност 
    public double Irradiation { get; set; }  // Слънчева радиация 
    public double Temperature { get; set; }  // Температура 
    public string Status { get; set; }       // Аномалии/Грешки 
}