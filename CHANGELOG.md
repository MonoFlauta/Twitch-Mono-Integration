# 0.0.16
## New
- Added UserId for EditorTwitchService
- Added Username for EditorTwitchService

# 0.0.15
## Fixed
- Fix typo in Poll events
- Fix option to subscribe to Poll
- Fix result calls for Poll

# 0.0.14
## Fixed
- Attempt to update code value for Auth

# 0.0.13
## Fixed
- Open URL more than once with the code if needed

# 0.0.12
## Fixed
- Re-offer authorization on logged out

# 0.0.11
## New
- Added support to create clips

# 0.0.10
## New 
- Auto create Scriptable Object with the permissions
- Take Auth values from ScriptableObject

# 0.0.9
## New
- Method Initialize and Initialized are separated in TwitchController
- Initialize method returns the code for authentication
## Fixed
- Fixed url for authentication

# 0.0.8

## Fixed
- Naming for User Id test in ChannelPointsRewardTestView
- How on finish was called for Poll 

# 0.0.7

## New
- Added support to set user id for ChannelPointRewardTestView
- Added support to set user id for ChannelFollowTestView
- Added ChannelSubscribeTestView

# 0.0.6

## New
- Added ChannelPointRewardTestView
- Added ChannelFollowTestView
- Added ChannelRaidTestView
- Limit Viewer count to be an integer

# 0.0.5

## Fixed
- Null reference for IObservable<long> ObserveViewerCount() in EditorTwitchService

# 0.0.4

## New
- Open test key code can be set in initialize now

## Fixed
- Fix null reference por Editor Test View reference

# 0.0.3

## New
- Added a way to editor test auth status
- Added a way to editor test view count

# 0.0.2

## New
- Added Set Custom Rewards support
- Added Clear Rewards support
- Added a way to subscribe to channel reward events
- Added a way to subscribe to channel follows
- Added a way to subscribe to subscribers
- Added a way to subscribe to hype train
- Added a way to subscribe to raids

## Fixed
- Adjusted folders for files
- Clean up comments

# 0.0.1

## New

- First commit