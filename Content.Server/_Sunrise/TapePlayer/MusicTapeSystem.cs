using Content.Shared._Sunrise.TapePlayer;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Log;

namespace Content.Server._Sunrise.TapePlayer;

public sealed class MusicTapeSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MusicTapeComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, MusicTapeComponent comp, MapInitEvent args)
    {
        try
        {
            var resolved = _audioSystem.ResolveSound(comp.Sound);
            var length = _audioSystem.GetAudioLength(resolved);
            comp.SongLengthSeconds = (float) length.TotalSeconds;
            Dirty(uid, comp);
        }
        catch (Exception e)
        {
            Log.Error($"Failed to calculate music tape length for entity {ToPrettyString(uid)}: sound={comp.Sound}. Exception: {e}");

            comp.SongLengthSeconds = 0f;
            Dirty(uid, comp);
        }
    }
}
