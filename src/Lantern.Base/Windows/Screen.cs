namespace Lantern.Windows;

public class Screen
{
    public double Scaling { get; }

    public LogisticRectangle Bounds { get; }

    public LogisticRectangle WorkingArea { get; }

    public PhysicsRectangle PhysicsBounds { get; }

    public PhysicsRectangle PhysicsWorkingArea { get; }

    public bool IsPrimary { get; }

    public Screen(double scaling, PhysicsRectangle bounds, PhysicsRectangle workingArea, bool isPrimary)
    {
        Scaling = scaling;
        Bounds = bounds.ToLogisticRectangle(scaling);
        WorkingArea = workingArea.ToLogisticRectangle(scaling);
        PhysicsBounds = bounds;
        PhysicsWorkingArea = workingArea;
        IsPrimary = isPrimary;
    }
}
