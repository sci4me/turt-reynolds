namespace Turt.Runtime {
    public interface ExecutionEnvironment {
        void PenUp();

        void PenDown();

        void TurnLeft(TurtInteger angle);

        void TurnRight(TurtInteger angle);

        void Go(TurtInteger distance);

        void SetColor(TurtInteger red, TurtInteger green, TurtInteger blue);

        void SetWidth(TurtInteger width);

        void Dot(TurtInteger size);

        void PushFrame();

        void PopFrame();

        Frame Frame { get; }
    }
}