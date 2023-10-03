﻿using GrundernaOOP.Locales;

namespace GrundernaOOP.Geometry;

public class Triangle
{
    private double[] _sides = new double[3];
    public double[] Sides
    {
        get => _sides;
        set
        {
            if (value[0] > 0 && value[1] > 0 && value[2] > 0)
                _sides = value;
            else
                throw new Exception("Attempted to assign non-positive value(s) to Sides. All values must be greater than 0.");
        }
    }
    public double[] Angles { get; }
    public TypeOfTriangle TriangleType { get; }

    public Triangle(double width, double height) // Overloaded constructor.
    {
        // This constructor assumes it's a right angled triangle and is scalene.
        var hypotenuse = GetHypotenuse(width, height);
        Sides = new[] { height, width, hypotenuse };
        Angles = GetAngles();
        TriangleType = GetTriangleType();
    }
    
    public Triangle(double sideA, double sideB, double sideC) // Overloaded constructor.
    {
        Sides = new[] { sideA, sideB, sideC };
        Angles = GetAngles();
        TriangleType = GetTriangleType();
    }

    private double GetHypotenuse(double baseOfTriangle, double heightOfTriangle)
    {
        // Simplified Pythagorean theorem: c = √[a² + b²]
        return Math.Sqrt(Math.Pow(heightOfTriangle, 2) + Math.Pow(baseOfTriangle, 2));
    }
    
    public double[] GetAngles()
    {
        /*
         * Calculate the angles using the law of cosines.
         * 
         * Since the length of every side is known, it is possible
         * to use the law of cosines to calculate every angle.
         */

        // a = arccos[(b² + c² - a²) / (2bc)]
        var angleA = Math.Acos((Math.Pow(Sides[1], 2) 
                                + Math.Pow(Sides[2], 2) 
                                - Math.Pow(Sides[0], 2)) 
                               / (2 * Sides[1] * Sides[2]));
        
        // b = arccos[(a² + c² - b²) / (2ac)]
        var angleB = Math.Acos((Math.Pow(Sides[0], 2) 
                                + Math.Pow(Sides[2], 2)
                                - Math.Pow(Sides[1], 2))
                               / (2 * Sides[0] * Sides[2]));
        
        // c = arccos[(a² + b² - c²) / (2ab)]
        var angleC = Math.Acos((Math.Pow(Sides[0], 2) 
                                + Math.Pow(Sides[1], 2) 
                                - Math.Pow(Sides[2], 2)) 
                               / (2 * Sides[0] * Sides[1]));

        // Convert angles from radians to degrees.
        angleA = angleA * 180 / Math.PI;
        angleB = angleB * 180 / Math.PI;
        angleC = angleC * 180 / Math.PI;

        return new[] { angleA, angleB, angleC };
    }
    
    public enum TypeOfTriangle
    {
        Equilateral,
        Isosceles,
        Scalene
    }

    public TypeOfTriangle GetTriangleType() // Determine what type the triangle is.
    {
        const double epsilon = 1E-12; // 1 * 10 to the power of -12 (0.000000000001).

        /*
         * An epsilon comparison is used rather than "==" due to possible loss of fraction caused
         * by how the values are stored in memory.
         */
        
        // If all sides are equal.
        if (Math.Abs(Sides[0] - Sides[1]) < epsilon &&
            Math.Abs(Sides[1] - Sides[2]) < epsilon)
            return TypeOfTriangle.Equilateral;

        // If two sides are equal.
        if (Math.Abs(Sides[0] - Sides[1]) < epsilon ||
            Math.Abs(Sides[1] - Sides[2]) < epsilon ||
            Math.Abs(Sides[0] - Sides[2]) < epsilon)
            return TypeOfTriangle.Isosceles;

        return TypeOfTriangle.Scalene; // The type must be scalene since no side is equal.
    }

    public double GetArea()
    {
        // Heron's formula: A = √[s(s - a)(s - b)(s - c)]
        var s = (Sides[0] + Sides[1] + Sides[2]) / 2; // Semiperimeter
        return Math.Sqrt(s * (s - Sides[0]) * (s - Sides[1]) * (s - Sides[2]));
    }
    
    public void PrintProperties()
    {
        Console.WriteLine($"{Lang.General["SideA"]}: {Math.Round(Sides[0], 2)}, " +
                          $"{Lang.General["SideB"]}: {Math.Round(Sides[1], 2)}, " +
                          $"{Lang.General["SideC"]}: {Math.Round(Sides[2], 2)}.");
        
        Console.WriteLine($"{Lang.General["TriangleType"]}: {Lang.TriangleTypes[TriangleType]}");
        Console.WriteLine($"{Lang.General["Area"]}: {Math.Round(GetArea(), 2)}");
        
        Console.WriteLine($"{Lang.General["AngleA"]}: {Math.Round(Angles[0], 2)}\u00b0, " +
                          $"{Lang.General["AngleB"]}: {Math.Round(Angles[1], 2)}\u00b0, " +
                          $"{Lang.General["AngleC"]}: {Math.Round(Angles[2], 2)}\u00b0.");
    }
}