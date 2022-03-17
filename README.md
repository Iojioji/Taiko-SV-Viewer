A tiny program that allows you to view an osu map's SV as a graph

You can check out other mode's maps. I haven't really tested those but it should work

- - - -

### TODOS
- [ ] Settings
    - [ ] Change graph colors
    - [ ] Change marker style (color, size, show/hide)
- [ ] Open multiple diffs
- [ ] Make barline gimmicks not show (unless they're affecting a hitobject)
- [ ] Tweak exact SV+time when you hover a point on the graph; it's a bit picky sometimes
- [x] Change marker size depending on zoom level
- [x] Add drag-and-drop to open diffs
- [x] Add auto-update thing

- - - -

## What it already does
- Open Beatmap Diffs
  - Either through File > Open or by dragging an .osu file into it
- Refresh an already open map to check out those new cool SV changes you've made
- Rough zoom in by selecting the are you want to zoom into
- Exact SV + time of an object if you hover your mouse over a point on the line
- Tells you when there's a pending update and handles updates (mostly; it'll ask if you wanna update first, in case there is an update available) on its own
- Can actually read beatmaps of other modes, not only osu!taiko (though I haven't tested it a lot so I dunno if it'll break if you open those haha)

- - - -

![](https://i.imgur.com/xZM3HVD.png "The program doing its thing")
