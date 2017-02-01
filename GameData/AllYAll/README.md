# AllYAll
One-Button Common Action Grouping

# LICENSE:
CC share-alike. Anyone is free to do anything they like with All Y'All's source, so long as they allow others to do the same.

# CHANGELOG:
- 0.10: Bugfix and code optimization, plus some changes to menus
  - Updated to 1.2.2
  - Fixed: Null Reference errors when deploying/retracting solar panels when static panels were present.
  - Buttons are more logical now, "retract all" is available on all extended panels, while "extend all" is on retracted ones. Changes for Radiators, Solar Panels, and Cargo Bays.
  - Minor changes: Does not appear on extended solar panels that cannot retract. Cargo Bays can be toggled while in motion to whatever they will eventually be.
- 0.9: MASSIVE rewrite with major additions, all by linuxgurugamer:
  - Updated to 1.2.1
  - Added Radiators, Drills, Reaction Wheels
  - Set up single-button functionality.
  - Was - in general - awesome.
- 0.8: Added functionality to the new 1.2 "Science Box."
  - "Perform All Science" even though it's not actually a science experiment, for ease of use.
  - "Reset All Science" - the good part.
    - Will reset all experiments whose data has not been collected.
    - Will reset Mystery Goo and Science Jr (and hopefully any modded science) if you have a scientist on board.
    - Usage: Pin the Science Box right-click menu, then click in turn "perform science", "collect data", and "reset science." Repeat in all biomes.
- 0.7: Recompile for KSP 1.2 (Prerelease, should work in final as well)
- 0.6: Quick fix for M.O.L.E. parts. They utilize ModuleScienceExperiment but shouldn't be auto-collected. I've hard coded to ignore them when triggering science.
- 0.5: Bugfix release
  - Fixed bug in science experiments: AYA allowed you to run Mystery Goo and Materials Bay science that had been collected, but not reset.
- 0.4: Bugfix release
  - Fixed bug in science experiments: AYA allowed you to perform surface samples in a command chair without having to upgrade R&D
  - Fixed bug in science experiments: AYA would only run the first experiment it came across in any given part. Now it will run each experiment in turn for all parts. Note this did not affect the stock game, but would affect any modded parts with multiple experiments.
  - Included all source files in source zip, to hopefully make it easier for others to build from source.
- 0.3: Science Experiments confirmed working.
- 0.2: Radiators confirmed working.
- 0.1: Solar panels confirmed working.

# GOALS:
Anything else I can think of that's similar (suggestions welcome!)

# THANKS:
Special thanks to NathanKell, Crzyrndm, DMagic, and Aelfhe1m, wasml, Diazo, and nightingale for code snippets and all around help.
I literally have no idea what I'm doing and without their help this mod would never have even started.
