# Avatar Motion Changer

This plugin is used to change the avatar motion in playable layers by NDMF(Non-Destructive Modular Framework).

## Usage

For example, Change `Fist` motion in `Left Hand` layer of `Gesture` animator. 

1. Create GameObject under your avatar.
2. Attach `AvatarMotionChanger` component to the GameObject.
3. Press `+` in the `Avatar Motion Changer` component.
4. Set the parameters,
    - `Playable Layer`: `Gesture`
    - `Layer`: `Left Hand`
    - `State`: `Fist`
    - `Motion`: Any motion object 
5. After Run, you can see the motion is changed.

## Acknowledgement

This software includes the part of following software.

- [bdunderscore/modular-avatar](https://github.com/bdunderscore/modular-avatar)

## License

MIT License

Copyright (c) 2024 Yutaka TSUMORI

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.