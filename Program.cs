using System;
using System.Drawing;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

public class SquareMatrix
{
    int[,] _matrix;
    public int[,] Value { get { return _matrix; } }
    static readonly Random s_random = new Random();
    public int MatrixSize;

    public SquareMatrix(int size)
    {

        try
        {
            _matrix = new int[size, size];
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        for (var RowOfMatrix = 0; RowOfMatrix < _matrix.GetLength(0); ++RowOfMatrix)
        {
            for (var ColumnOfMatrix = 0; ColumnOfMatrix < _matrix.GetLength(1); ++ColumnOfMatrix)
            {
                int RandomNumber = s_random.Next(100);
                _matrix[RowOfMatrix, ColumnOfMatrix] = RandomNumber;
            }
        }
        MatrixSize = _matrix.GetLength(0);
    }

    // Далее перегрузки:
    public static SquareMatrix operator +(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        int Rows = matrix1.Value.GetLength(0);
        int Columns = matrix1.Value.GetLength(1);

        if (Rows != matrix2.Value.GetLength(0) || Columns != matrix2.Value.GetLength(1))
        {
            throw new SquareMatrixDimensionsException("Матрицы разного размера.", Rows, matrix2.Value.GetLength(0));
        }

        int[,] MatrixResultOfOperation = new int[Rows, Columns];
        for (int indexOfRow = 0; indexOfRow < Rows; ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < Columns; ++indexOfColumn)
            {
                MatrixResultOfOperation[indexOfRow, indexOfColumn] = matrix1.Value[indexOfRow, indexOfColumn] + matrix2.Value[indexOfRow, indexOfColumn];
            }
        }
        SquareMatrix NewMatrix = new SquareMatrix(Rows);
        NewMatrix._matrix = MatrixResultOfOperation;
        return NewMatrix;
    }

    public static SquareMatrix operator *(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        int RowCount1 = matrix1.Value.GetLength(0);
        int ColumnCount1 = matrix1.Value.GetLength(1);
        int RowCount2 = matrix2.Value.GetLength(0);
        int ColumnCount2 = matrix2.Value.GetLength(1);
        if (ColumnCount1 != RowCount2)
        {
            throw new SquareMatrixDimensionsException("Нельзя перемножить матрицы. Количество столбцов матрицы 1 не равно количеству строк матрицы 2.", matrix1.Value.GetLength(0), matrix2.Value.GetLength(0));
        }
        int[,] MatrixResultOfOperation = new int[RowCount1, ColumnCount2];

        for (int IndexOfRow1 = 0; IndexOfRow1 < RowCount1; ++IndexOfRow1)
        {
            for (int IndexOfColumn2 = 0; IndexOfColumn2 < ColumnCount2; ++IndexOfColumn2)
            {
                for (int IndexOfColumn1 = 0; IndexOfColumn1 < ColumnCount1; ++IndexOfColumn1)
                {
                    MatrixResultOfOperation[IndexOfRow1, IndexOfColumn2] += matrix1.Value[IndexOfRow1, IndexOfColumn1] * matrix2.Value[IndexOfColumn1, IndexOfColumn2];
                }
            }
        }

        SquareMatrix NewMatrix = new SquareMatrix(RowCount1);
        NewMatrix._matrix = MatrixResultOfOperation;
        return NewMatrix;

    }

    public static bool operator >(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        for (int indexOfRow = 0; indexOfRow < matrix1.Value.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < matrix1.Value.GetLength(1); ++indexOfColumn)
            {
                if (matrix1.Value[indexOfRow, indexOfColumn] <= matrix2.Value[indexOfRow, indexOfColumn])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool operator <(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        for (int indexOfRow = 0; indexOfRow < matrix1.Value.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < matrix1.Value.GetLength(1); ++indexOfColumn)
            {
                if (matrix1.Value[indexOfRow, indexOfColumn] >= matrix2.Value[indexOfRow, indexOfColumn])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool operator >=(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        for (int indexOfRow = 0; indexOfRow < matrix1.Value.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < matrix1.Value.GetLength(1); ++indexOfColumn)
            {
                if (matrix1.Value[indexOfRow, indexOfColumn] < matrix2.Value[indexOfRow, indexOfColumn])
                {
                    return false;  // в случае, если хоть один элемент меньше, вернем false
                }
            }
        }
        return true;  // если все элементы больше или равны, вернем true
    }

    public static bool operator <=(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        for (int indexOfRow = 0; indexOfRow < matrix1.Value.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < matrix1.Value.GetLength(1); ++indexOfColumn)
            {
                if (matrix1.Value[indexOfRow, indexOfColumn] > matrix2.Value[indexOfRow, indexOfColumn])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool operator ==(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        if (ReferenceEquals(matrix1, matrix2))
        {
            return true;
        }
        if (ReferenceEquals(matrix1, null) || ReferenceEquals(matrix2, null))
        {
            return false;
        }

        if (matrix1.Value.GetLength(0) != matrix2.Value.GetLength(0) || matrix1.Value.GetLength(1) != matrix2.Value.GetLength(1))
        {
            return false;
        }

        for (int indexOfRow = 0; indexOfRow < matrix1.Value.GetLength(0); indexOfRow++)
        {
            for (int indexOfColumn = 0; indexOfColumn < matrix1.Value.GetLength(1); indexOfColumn++)
            {
                if (matrix1.Value[indexOfRow, indexOfColumn] != matrix2.Value[indexOfRow, indexOfColumn])
                {
                    return false;
                }
            }
        }
        return true;
    }
    public static bool operator !=(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        return !(matrix1 == matrix2);
    }

    // ПРОЧИЕ МЕТОДЫ

    public static explicit operator int[,](SquareMatrix inputMatrix) // Приведение типов
    {
        return inputMatrix._matrix;
    }

    public double CalculateDeterminant(int[,] matrix)
    {
        int lengthOfMatrix = matrix.GetLength(0);
        if (lengthOfMatrix == 2)
        {
            return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        }
        double Determinant = 0;
        for (int indexOfRow = 0; indexOfRow < lengthOfMatrix; ++indexOfRow)
        {
            Determinant += Math.Pow(-1, indexOfRow) * matrix[0, indexOfRow] * CalculateDeterminant(GetSubMatrix(matrix, 0, indexOfRow));
        }
        return Determinant;
    }

    private int[,] GetSubMatrix(int[,] matrix, int excludeRow, int excludeCol)
    {
        int lengthOfMatrix = matrix.GetLength(0);
        int[,] SubMatrix = new int[lengthOfMatrix - 1, lengthOfMatrix - 1];
        int Row = -1;
        for (int indexOfRow = 0; indexOfRow < lengthOfMatrix; ++indexOfRow)
        {
            if (indexOfRow == excludeRow)
            {
                continue;
            }
            ++Row;
            int Column = -1;
            for (int indexOfColumn = 0; indexOfColumn < lengthOfMatrix; ++indexOfColumn)
            {
                if (indexOfColumn == excludeCol) continue;
                SubMatrix[Row, ++Column] = matrix[indexOfRow, indexOfColumn];
            }
        }
        return SubMatrix;
    }

    public double[,] InvertMatrix(int[,] matrix)
    {
        int lengthOfMatrix = matrix.GetLength(0);

        // Проверяем, существует ли обратная матрица (детерминант не равен 0)
        double Determinant = CalculateDeterminant(matrix);
        if (Determinant == 0)
        {
            throw new SquareMatrixDimensionsException("Матрица вырожденная, обратной матрицы не существует.");
        }
        // Создаем обратную матрицу путем деления каждого элемента присоединенной матрицы на детерминант
        double[,] InverseMatrix = new double[lengthOfMatrix, lengthOfMatrix];
        if (lengthOfMatrix == 2)
        {
            double multiplier = 1.0 / Determinant;

            InverseMatrix[0, 0] = matrix[1, 1] * multiplier;
            InverseMatrix[0, 1] = -matrix[0, 1] * multiplier;
            InverseMatrix[1, 0] = -matrix[1, 0] * multiplier;
            InverseMatrix[1, 1] = matrix[0, 0] * multiplier;
            return InverseMatrix;
        }

        // Находим присоединенную матрицу
        int[,] AdjointMatrix = GetAdjointMatrix(matrix);

        for (int indexOfRow = 0; indexOfRow < lengthOfMatrix; ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < lengthOfMatrix; ++indexOfColumn)
            {
                InverseMatrix[indexOfRow, indexOfColumn] = AdjointMatrix[indexOfRow, indexOfColumn] / Determinant;
            }
        }

        return InverseMatrix;
    }

    private int[,] GetAdjointMatrix(int[,] matrix)
    {
        int lengthOfMatrix = matrix.GetLength(0);
        int[,] Adjoint = new int[lengthOfMatrix, lengthOfMatrix];

        for (int indexOfRow = 0; indexOfRow < lengthOfMatrix; ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < lengthOfMatrix; ++indexOfColumn)
            {
                Adjoint[indexOfRow, indexOfColumn] = (int)Math.Pow(-1, indexOfRow + indexOfColumn) * (int)CalculateDeterminant(GetSubMatrix(matrix, indexOfRow, indexOfColumn));
            }
        }
        // Транспонируем присоединенную матрицу для получения окончательной присоединенной матрицы
        return TransposeMatrix(Adjoint);
    }
    int[,] TransposeMatrix(int[,] matrix)
    {
        int Rows = matrix.GetLength(0);
        int Columns = matrix.GetLength(1);

        int[,] TransposedMatrix = new int[Columns, Rows];

        for (int indexOfRow = 0; indexOfRow < Columns; ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < Rows; ++indexOfColumn)
            {
                TransposedMatrix[indexOfRow, indexOfColumn] = matrix[indexOfColumn, indexOfRow];
            }
        }

        return TransposedMatrix;
    }

    public SquareMatrix TransposeMatrix(SquareMatrix matrix)
    {
        int Rows = matrix._matrix.GetLength(0);
        int Columns = matrix._matrix.GetLength(1);

        SquareMatrix TransposedMatrix = new SquareMatrix(Rows);

        for (int indexOfRow = 0; indexOfRow < Columns; ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < Rows; ++indexOfColumn)
            {
                TransposedMatrix._matrix[indexOfRow, indexOfColumn] = matrix._matrix[indexOfColumn, indexOfRow];
            }
        }

        return TransposedMatrix;
    }

    public void PrintMatrix()
    {
        for (int indexOfRow = 0; indexOfRow < _matrix.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < _matrix.GetLength(1); ++indexOfColumn)
            {
                Console.Write(_matrix[indexOfRow, indexOfColumn] + " ");
            }
            Console.WriteLine();
        }
    }

    public void PrintDeterminant()
    {
        Double Determinant = CalculateDeterminant(_matrix);
        Console.WriteLine(Determinant);
    }

    public void PrintInvertMatrix()
    {
        double[,] InvertedMatrix = InvertMatrix(_matrix);
        int Rows = InvertedMatrix.GetLength(0);
        int Columns = InvertedMatrix.GetLength(1);

        for (int indexOfRow = 0; indexOfRow < Rows; ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < Columns; ++indexOfColumn)
            {
                Console.Write(InvertedMatrix[indexOfRow, indexOfColumn].ToString("0.000") + "\t");
            }
            Console.WriteLine();
        }
    }

    public void PrintClone()
    {
        SquareMatrix СlonedMatrix = (SquareMatrix)Clone();
        СlonedMatrix.PrintMatrix();
    }

    public void PrintHashCode()
    {
        var HashCode = _matrix.GetHashCode();
        Console.WriteLine(HashCode);
    }


    // Методы ToString(), CompareTo(), Equals(), GetHashCode():

    public override string ToString()
    {
        string MatrixString = "";
        for (int indexOfRow = 0; indexOfRow < Value.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < Value.GetLength(1); ++indexOfColumn)
            {
                MatrixString += Value[indexOfRow, indexOfColumn] + "\t";
            }
            MatrixString += "\n";
        }
        return MatrixString;
    }

    public int CompareTo(SquareMatrix other)
    {
        if (other == null)
        {
            return 1; // Матрица не существует, поэтому текущая матрица больше
        }
        if (this.Value.GetLength(0) != other.Value.GetLength(0) || this.Value.GetLength(1) != other.Value.GetLength(1))
        {
            return -1;
        }
        return 0;
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as SquareMatrix);
    }

    public bool Equals(SquareMatrix other)
    {
        if (other == null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (this.Value.GetLength(0) != other.Value.GetLength(0) || this.Value.GetLength(1) != other.Value.GetLength(1))
        {
            return false;
        }
        for (int indexOfRow = 0; indexOfRow < this.Value.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < this.Value.GetLength(1); ++indexOfColumn)
            {
                if (this.Value[indexOfRow, indexOfColumn] != other.Value[indexOfRow, indexOfColumn])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int Hash = 17;

            for (int indexOfRow = 0; indexOfRow < Value.GetLength(0); ++indexOfRow)
            {
                for (int indexOfColumn = 0; indexOfColumn < Value.GetLength(1); ++indexOfColumn)
                {
                    Hash = Hash * 23 + Value[indexOfRow, indexOfColumn].GetHashCode();
                }
            }

            return Hash;
        }
    }

    // Паттерн "прототип" 
    public object Clone()
    {
        SquareMatrix NewMatrix = new SquareMatrix(_matrix.GetLength(0));
        for (int indexOfRow = 0; indexOfRow < _matrix.GetLength(0); ++indexOfRow)
        {
            for (int indexOfColumn = 0; indexOfColumn < _matrix.GetLength(1); ++indexOfColumn)
            {
                NewMatrix._matrix[indexOfRow, indexOfColumn] = _matrix[indexOfRow, indexOfColumn];
            }
        }
        return NewMatrix;
    }
    public int Trace()
    {
        int trace = 0;
        for (int sizeOfMatrix = 0; sizeOfMatrix < _matrix.GetLength(0); ++sizeOfMatrix)
        {
            trace += _matrix[sizeOfMatrix, sizeOfMatrix];
        }
        return trace;
    }
    public void PrintTrace()
    {
        int trace = Trace();
        Console.WriteLine(trace); 
    }

    public delegate SquareMatrix MatrixDiagonalizer(SquareMatrix matrix);

    MatrixDiagonalizer diagonalizer = (SquareMatrix matrix) => 
    {
        int matrixLength = matrix.Value.GetLength(0);
        SquareMatrix diagonalizedMatrix = new SquareMatrix(matrixLength); // Создаем новую матрицу для изменений
        int[,] matrixValues = matrix.Value;

        for (int rowIndex = 0; rowIndex < matrixLength; ++rowIndex)
        {
            for (int colIndex = 0; colIndex < matrixLength; ++colIndex)
            {
                if (rowIndex == colIndex)
                {
                    diagonalizedMatrix.Value[rowIndex, colIndex] = matrixValues[rowIndex, colIndex];
                }
                else
                {
                    diagonalizedMatrix.Value[rowIndex, colIndex] = 0;
                }
            }
        }

        return diagonalizedMatrix;
    };

}

class SquareMatrixDimensionsException : Exception
{
    public int SizeOfMatrix1 { get; }
    public int SizeOfMatrix2 { get; }

    public SquareMatrixDimensionsException()
    {
    }

    public SquareMatrixDimensionsException(string message)
      : base(message)
    {
    }

    public SquareMatrixDimensionsException(string message, int sizeOfMatrix1, int sizeOfMatrix2)
      : base(message)
    {
        SizeOfMatrix1 = sizeOfMatrix1;
        SizeOfMatrix2 = sizeOfMatrix2;
    }
}

public abstract class IEvent
{
    public string EventType { get; set; }
}

class Sum : IEvent
{
    public Sum() { EventType = "Сложение"; }
}

class Product : IEvent
{
    public Product() { EventType = "Произведение"; }
}

class Trace : IEvent
{
    public Trace() { EventType = "След"; }
}

class Determinant : IEvent
{
    public Determinant() { EventType = "Найти определитель"; }
}

class MatrixInvert : IEvent
{
    public MatrixInvert() { EventType = "Найти обратную матрицу"; }
}

public abstract class BaseHandler
{
    private SquareMatrix _matrix1;
    private SquareMatrix _matrix2;

    protected BaseHandler(SquareMatrix matrix1, SquareMatrix matrix2)
    {
        this._matrix1 = matrix1;
        this._matrix2 = matrix2;
    }

    protected BaseHandler Next { get; set; }
    protected IEvent PrivateEvent { get; set; }

    public virtual void Handle(IEvent ev)
    {
        if (PrivateEvent.GetType() == ev.GetType())
        {
            Console.WriteLine("{0} обработано", PrivateEvent.EventType);
            HandleEvent(ev);
        }
        else
        {
            Console.WriteLine("Отправка события следующему обработчику...");
            Next?.Handle(ev);
        }
    }

    protected void SetNextHandler(BaseHandler newHandler)
    {
        Next = newHandler;
    }

    protected abstract void HandleEvent(IEvent ev);
}

class InvertHandler : BaseHandler
{
    private SquareMatrix _matrix1;
    private SquareMatrix _matrix2;

    public InvertHandler(SquareMatrix matrix1, SquareMatrix matrix2) : base(matrix1, matrix2)
    {
        PrivateEvent = new MatrixInvert();
        SetNextHandler(null);

        this._matrix1 = matrix1;
        this._matrix2 = matrix2;
    }

    protected override void HandleEvent(IEvent ev)
    {
        Console.WriteLine("Транспонирование первой матрицы:");
        _matrix1.PrintInvertMatrix();
        Console.WriteLine("Транспонирование второй матрицы:");
        _matrix1.PrintInvertMatrix();
    }
}

class DeterminantHandler : BaseHandler
{
    private SquareMatrix _matrix1;
    private SquareMatrix _matrix2;

    public DeterminantHandler(SquareMatrix matrix1, SquareMatrix matrix2) : base(matrix1, matrix2)
    {
        PrivateEvent = new Determinant();
        SetNextHandler(new InvertHandler(matrix1, matrix2));

        this._matrix1 = matrix1;
        this._matrix2 = matrix2;
    }

    protected override void HandleEvent(IEvent ev)
    {
        Console.WriteLine("Определитель первой матрицы:");
        _matrix1.PrintDeterminant();
        Console.WriteLine("Определитель второй матрицы:");
        _matrix1.PrintDeterminant();
    }
}

class TraceHandler : BaseHandler
{
    private SquareMatrix _matrix1;
    private SquareMatrix _matrix2;

    public TraceHandler(SquareMatrix matrix1, SquareMatrix matrix2) : base(matrix1, matrix2)
    {
        PrivateEvent = new Trace();
        SetNextHandler(new DeterminantHandler(matrix1, matrix2));

        this._matrix1 = matrix1;
        this._matrix2 = matrix2;
    }
    protected override void HandleEvent(IEvent ev)
    {
        Console.WriteLine("След первой матрицы:");
        _matrix1.PrintTrace();
        Console.WriteLine("След второй матрицы:");
        _matrix2.PrintTrace();
    }
}

class ProductHandler : BaseHandler
{
    private SquareMatrix _matrix1;
    private SquareMatrix _matrix2;

    public ProductHandler(SquareMatrix matrix1, SquareMatrix matrix2) : base(matrix1, matrix2)
    {
        PrivateEvent = new Product();
        SetNextHandler(new TraceHandler(matrix1, matrix2));

        this._matrix1 = matrix1;
        this._matrix2 = matrix2;
    }

    protected override void HandleEvent(IEvent ev)
    {
        try
        {
            Console.WriteLine(_matrix1 * _matrix2);

        }
        catch (SquareMatrixDimensionsException exception)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            Console.WriteLine($"Определись с размером! Либо: {exception.SizeOfMatrix1} Либо: {exception.SizeOfMatrix2}");
            Console.WriteLine();
        }
    }
}

class SumHandler : BaseHandler
{
    private SquareMatrix _matrix1;
    private SquareMatrix _matrix2;
    public SumHandler(SquareMatrix matrix1, SquareMatrix matrix2) : base(matrix1, matrix2)
    {
        PrivateEvent = new Sum();
        SetNextHandler(new ProductHandler(matrix1, matrix2));

        this._matrix1 = matrix1;
        this._matrix2 = matrix2;
    }

    protected override void HandleEvent(IEvent ev)
    {
        try
        {
            Console.WriteLine(_matrix1 + _matrix2);

        }
        catch (SquareMatrixDimensionsException exception)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            Console.WriteLine($"Определись с размером! Либо: {exception.SizeOfMatrix1} Либо: {exception.SizeOfMatrix2}");
            Console.WriteLine();
        }
    }
}

public class ChainApp
{
    private BaseHandler _calculationHandler;

    public ChainApp(BaseHandler calculationHandler)
    {
        this._calculationHandler = calculationHandler;
    }

    public void Execute(int choice, SquareMatrix matrix1, SquareMatrix matrix2)
    {
        IEvent ev = null;

        // Создание экземпляра события в зависимости от выбора
        switch (choice)
        {
            case 1:
                ev = new Sum();
                break;
            case 2:
                ev = new Product();
                break;
            case 3:
                ev = new Trace();
                break;
            case 4:
                ev = new Determinant();
                break;
            case 5:
                ev = new MatrixInvert();
                break;
            default:
                Console.WriteLine("Такого не знаем, ты ошибся номером, друг.");
                return;
        }

        _calculationHandler.Handle(ev);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("НОТА БЕНЕ: Матрицы рандомные! Ручками не повертишь - сорян!\n");

        string InputSizeOfMatrix1 = "";
        string InputSizeOfMatrix2 = "";
        int SizeOfMatrix1 = 0;
        int SizeOfMatrix2 = 0;
        bool InputSuccess = false;

        while (!InputSuccess)
        {
            try
            {
                InputSizeOfMatrix1 = ""; // обнуление для корректной работы обработчика исключений
                InputSizeOfMatrix1 = "";
                SizeOfMatrix1 = 0;
                SizeOfMatrix2 = 0;

                Console.WriteLine("Введите размер первой квадратной матрицы (одно число):");
                InputSizeOfMatrix1 = (Console.ReadLine());
                SizeOfMatrix1 = int.Parse(InputSizeOfMatrix1);

                Console.WriteLine("Введите размер второй квадратной матрицы (одно число):");
                InputSizeOfMatrix2 = (Console.ReadLine());
                SizeOfMatrix2 = int.Parse(InputSizeOfMatrix2);
                if (SizeOfMatrix1 != 0 && SizeOfMatrix2 != 0)
                {
                    InputSuccess = true;
                }
                if (SizeOfMatrix1 <= 0 || SizeOfMatrix2 <= 0)
                {
                    try
                    {
                        throw new Exception("Размер должен быть больше 0");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nОшибка: " + ex.Message + "\n");
                        InputSuccess = false;
                    }

                }
            }
            catch (FormatException)
            {
                if (InputSizeOfMatrix1.Length == 0)
                {
                    Console.WriteLine($"Ошибка: Вы ничего не ввели для 1 матрицы!");
                }
                else if (InputSizeOfMatrix1.Length != 0 && SizeOfMatrix1 == 0)
                {
                    Console.WriteLine($"Ошибка: Вы ввели буквы для 1 матрицы!");
                }
                else if (InputSizeOfMatrix2.Length == 0)
                {
                    Console.WriteLine($"Ошибка: Вы ничего не ввели для 2 матрицы!");
                }
                else if (InputSizeOfMatrix2.Length != 0 && SizeOfMatrix2 == 0)
                {
                    Console.WriteLine($"Ошибка: Вы ввели буквы для 2 матрицы!");
                }
                else if (SizeOfMatrix1 == 0 || SizeOfMatrix2 == 0)
                {
                    Console.WriteLine($"Ошибка: Размер должен быть больше нуля!");
                }
                Console.WriteLine();
            }
        }

        SquareMatrix ExampleMatrix1 = new SquareMatrix(SizeOfMatrix1);
        SquareMatrix ExampleMatrix2 = new SquareMatrix(SizeOfMatrix2);

        Console.WriteLine();
        Console.WriteLine("Первая матрица:");
        ExampleMatrix1.PrintMatrix();
        Console.WriteLine("Вторая матрица:");
        ExampleMatrix2.PrintMatrix();
        Console.WriteLine();

        BaseHandler SumHandler = new SumHandler(ExampleMatrix1, ExampleMatrix2);
        ChainApp ChainApp = new ChainApp(SumHandler);

        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1. Сложение");
        Console.WriteLine("2. Произведение");
        Console.WriteLine("3. След");
        Console.WriteLine("4. Найти определитель");
        Console.WriteLine("5. Найти обратную матрицу");

        int choice;
        if (int.TryParse(Console.ReadLine(), out choice))
        {
            ChainApp.Execute(choice, ExampleMatrix1, ExampleMatrix2);
        }
        else
        {
            Console.WriteLine("Буквы запрещены.");
        }
    }
}
