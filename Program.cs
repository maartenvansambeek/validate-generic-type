using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

var app = builder.Build();

app.MapGet("/int", ([Required] MultiValue<int> multiValue) => $"Received multi-value {multiValue.Value} ({multiValue.Type}).");
app.MapGet("/string", ([Required] MultiValue<string> multiValue) => $"Received multi-value {multiValue.Value} ({multiValue.Type}).");

app.Run();

/// <summary>
/// A generic type that holds a value and its type.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="Type">The name of the type.</param>
/// <param name="Value">The value.</param>
public record MultiValue<T>([Required] string Type, [Required] T Value)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiValue{T}"/> class.
    /// </summary>
    /// <param name="value"></param>
    public MultiValue(T value) : this(typeof(T).Name, value)
    {
    }

    /// <summary>
    /// Parses the string value into a MultiValue<T> object.
    /// </summary>
    /// <param name="value">The value to parse.</param>
    /// <param name="result">The result.</param>
    /// <returns>A value indicating whether the value was parsed successfully.</returns>
    public static bool TryParse(string value, out MultiValue<T> result)
    {
        if (typeof(T) == typeof(int))
        {
            if (int.TryParse(value, out var intValue))
            {
                result = new MultiValue<T>((T)(object)intValue);
                return true;
            }
        }
        else if (typeof(T) == typeof(string))
        {
            result = new MultiValue<T>((T)(object)value);
            return true;
        }

        result = default!;
        return false;
    }
}
