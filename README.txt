Use QuickSave for saving
https://assetstore.unity.com/packages/tools/utilities/quick-save-107676

Use DigitalRuby.SoundManager for sound
https://assetstore.unity.com/packages/tools/audio/sound-manager-audio-sound-and-music-manager-for-unity-56087

// ------------------------------------------------------------------------------------------------

This is an adventure map(graph). o - node in map. Every node has NextNodes list property, set in Editor.
At the beginning, all nodes are NotPassed.

o - o - o \     / - o - \
           o - o -- o -- o
o - o - o /     \ o - o /

Start game from left to right, first 2 nodes become Available (you can click it).

A - o - o \     / - o - \
           o - o -- o -- o
A - o - o /     \ o - o /

Click on Available node, it becomes Current. The second branch becomes NotAvailable, because we can go only left to right.

C - o - o \     / - o - \
           o - o -- o -- o
N - N - N /     \ o - o /

After event in Current node, this node is Passed. Next node(s) become Available.

P - A - o \     / - o - \
           o - o -- o -- o
N - N - N /     \ o - o /