using Content.Server._Misfits.Speech.Components;
using Content.Server.Speech;
using Content.Server.Speech.EntitySystems;
using Robust.Shared.Random;
using System.Text.RegularExpressions;

namespace Content.Server._Misfits.Speech.EntitySystems;

public sealed class GermanAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly ReplacementAccentSystem _replacement = default!;

    private static readonly Regex RegexThe = new(@"\bthe\b", RegexOptions.IgnoreCase);
    private static readonly Regex RegexThis = new(@"\bthis\b", RegexOptions.IgnoreCase);
    private static readonly Regex RegexThat = new(@"\bthat\b", RegexOptions.IgnoreCase);
    private static readonly Regex RegexThere = new(@"\bthere\b", RegexOptions.IgnoreCase);
    private static readonly Regex RegexThey = new(@"\bthey\b", RegexOptions.IgnoreCase);

    private static readonly Regex RegexThSoft = new(@"th", RegexOptions.IgnoreCase);
    private static readonly Regex RegexWStart = new(@"\bw", RegexOptions.IgnoreCase);

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GermanAccentComponent, AccentGetEvent>(OnAccentGet);
    }

    public string Accentuate(string message)
    {
        var words = message.Split(' ');
        var accentuatedWords = new List<string>();

        foreach (var word in words)
        {
            // Apply FTL dictionary replacements first.
            var accentuatedWord = _replacement.ApplyReplacements(word, "n14German");

            // If the word was not replaced by the dictionary, apply German-style phonetics.
            if (accentuatedWord == word)
            {
                accentuatedWord = ApplyRegexReplacements(accentuatedWord);
            }

            accentuatedWords.Add(accentuatedWord);
        }

        var result = string.Join(" ", accentuatedWords);

        // Add occasional German filler to the end of the sentence.
        var roll = _random.NextDouble();

        if (roll < 0.03)
            result += ", ja?";
        else if (roll < 0.06)
            result += ", nein?";
        else if (roll < 0.09)
            result += ". Wunderbar";

        return result;
    }

    private static string ApplyRegexReplacements(string word)
    {
        // Common German-accent word sounds.
        word = RegexThe.Replace(word, "ze");
        word = RegexThis.Replace(word, "zis");
        word = RegexThat.Replace(word, "zat");
        word = RegexThere.Replace(word, "zere");
        word = RegexThey.Replace(word, "zey");

        // General sound changes.
        word = RegexThSoft.Replace(word, "z");
        word = RegexWStart.Replace(word, "v");

        return word;
    }

    private void OnAccentGet(EntityUid uid, GermanAccentComponent component, AccentGetEvent args)
    {
        args.Message = Accentuate(args.Message);
    }
}
