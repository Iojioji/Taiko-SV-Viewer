A tiny program that allows you to view an osu map's SV as a graph

You can check out other mode's maps. I haven't really tested those but it should work

- - - -

### TODOS
- [ ] Settings
    - [ ] Change graph colors
    - [ ] Change marker style (color, size, show/hide)
- [ ] Open multiple diffs
- [ ] Tweak exact SV+time when you hover a point on the graph; it's a bit picky sometimes
- [x] Let different SV modifying mods affect the graph
- [x] Make barline gimmicks not show (unless they're affecting a hitobject)
- [x] Change marker size depending on zoom level
- [x] Add drag-and-drop to open diffs
- [x] Add auto-update thing

- - - -

## What it already does
- Open Beatmap Diffs
  - Either through File > Load, by dragging a .osu file into it or by having osu! open (lazer makes some funny things happen)
- Show graph being affected by HR or EZ, these can be set manually or read automatically if osu! is open
- Refresh an already open map to check out those new cool SV changes you've made (you can also press F5)
- Rough zoom in by selecting the are you want to zoom into (you can un-zoom by clicking the "(-)" symbol on the upper-left corner for Y axis or the lower-left corner for the X axis)
- Exact SV + time of an object if you hover your mouse over a point on the line
- Tells you when there's a pending update and handles updates (mostly; it'll ask if you wanna update first, in case there is an update available) on its own
- Can actually read beatmaps of other modes, not only osu!taiko (though I haven't tested it a lot so I dunno if it'll break if you open those haha)

- - - -

![](https://i.imgur.com/xZM3HVD.png "The program doing its thing")

The X axis indicates the time (in seconds)

The Y axis indicates the SV; a 100 bpm map with x1 SV will have 100 SV, the same 100 bpm map with x1.5 SV will have 150 SV

The green line indicates non-kiai sections, the orange line indicates kiai sections
