﻿using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using UnityEngine;
using AuthenticationValues = ExitGames.Client.Photon.Chat.AuthenticationValues;

using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;


/// <summary>
/// Simple Chat Server Gui to be put on a separate GameObject. Provides a global chat (in lobby) and a room chat (in room).
/// </summary>
/// <remarks>
/// This script flags it's GameObject with DontDestroyOnLoad(). Make sure this is OK in your case.
/// 
/// The Chat Server API in ChatClient basically lets you create any number of channels. 
/// You just have to name them. Example: "gc" for Global Channel or for rooms: "rc"+RoomName.GetHashCode()
/// 
/// This simple demo sends in a global chat when in lobby and in room channel when in room.
/// Create a more elaborate UI to let players chat in either channel while in room or send private messages.
/// 
/// Names of users are set in Authenticate. That should be unique so users can actually get their messages.
/// 
/// 
/// Workflow: 
/// Create ChatClient, Connect to a server with your AppID, Authenticate the user (apply a unique name)
/// and subscribe to some channels. 
/// Subscribe a channel before you publish to that channel!
/// 
/// 
/// Note: 
/// Don't forget to call ChatClient.Service(). Might later on be integrated into PUN but for now don't forget.
/// </remarks>
/// <credit>
/// Code is taken from Photon Unity Networking ChatGui demo and editted and changed to fit our application.
/// </credit>
public class ChatTest : MonoBehaviour, IChatClientListener
{
	// Chat update static variables
	static float FilScreenWidth = 0.328125f;
	static float FilScreenHeight = 0.141666667f;

	public string ChatAppId;                    // set in inspector. Your Chat AppId (don't mix it with Realtime/Turnbased Apps).
	public string[] ChannelsToJoinOnConnect;    // set in inspector. Demo channels to join automatically.
	public int HistoryLengthToFetch;            // set in inspector. Up to a certain degree, previously sent messages can be fetched for context.
	public bool DemoPublishOnSubscribe;         // set in inspector. For demo purposes, we publish a default text to any subscribed channel.
	
	public string UserName { get; set; }
	
	private ChatChannel selectedChannel;
	private string selectedChannelName;     // mainly used for GUI/input
	private int selectedChannelIndex = 0;   // mainly used for GUI/input
	bool doingPrivateChat;
	
	public ChatClient chatClient;
	
	// GUI stuff:
	public Rect GuiRect = new Rect(15, 900, 630, 153);
	public bool IsVisible = true;
	public bool AlignBottom = false;
	public bool FullScreen = false;
	
	private string inputLine = "";
	private string userIdInput = "";
	private Vector2 scrollPos = Vector2.zero;
	private static string WelcomeText = "Welcome to chat.\\help lists commands.";
	private static string HelpText = "\n\\subscribe <list of channelnames> subscribes channels.\n\\unsubscribe <list of channelnames> leaves channels.\n\\msg <username> <message> send private message to user.\n\\clear clears the current chat tab. private chats get closed.\n\\help gets this help message.";

	private static ChatTest instance;
	public static ChatTest Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<ChatTest>();
			}
			return instance;
		}
	}
	
	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else if (instance != this)
		{
			Debug.LogWarning("Destroying duplicate instance of "+this.gameObject+". ChatTest applies DontDestroyOnLoad() to this GameObject.");
			Destroy(this.gameObject);
		}
	}
	
	
	public void StartMe()
	{
		Application.runInBackground = true; // this must run in background or it will drop connection if not focussed.

		this.UserName = PhotonNetwork.player.name;

		if (string.IsNullOrEmpty(this.UserName))
		{
			this.UserName = "user" + Environment.TickCount%99; //made-up username CHANGE NEEDED
		}
		
		chatClient = new ChatClient(this);
		chatClient.Connect(ChatAppId, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(this.UserName));		

		if (this.AlignBottom)
		{
			this.GuiRect.y = Screen.height - this.GuiRect.height;
		}
		if (this.FullScreen)
		{
			this.GuiRect.x = 0;
			this.GuiRect.y = 0;
			this.GuiRect.width = Screen.width;
			this.GuiRect.height = Screen.height;
		}

	}
	
	/// <summary>To avoid the Editor becoming unresponsive, disconnect all Photon connections in OnApplicationQuit.</summary>
	public void OnApplicationQuit()
	{
		if (this.chatClient != null)
		{
			this.chatClient.Disconnect();
		}
	}
	
	/// <summary>To avoid the Editor becoming unresponsive, disconnect the Chat connection.</summary>
	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			if (chatClient != null)
			{
				chatClient.Disconnect();
			}
		}
	}

	/**
	 * Handle Chat scaling 
	 */
	public void Update()
	{
		if (this.chatClient != null)
		{
			this.chatClient.Service();  
			// make sure to call this regularly! it limits effort internally, so calling often is ok!
		}

		float rectWidth = FilScreenWidth * Screen.width;
		float rectHeight = FilScreenHeight * Screen.height;

		float rectX = 0.0078125f * Screen.width;
		float rectY = 0.833333333f * Screen.height;

		float whRatio = 0.24285714285f;

		// Check if height is wrong
		if (rectHeight > (rectWidth * whRatio)) {
			rectY = rectY + (rectHeight - (rectWidth * whRatio));
			rectHeight = (rectWidth * whRatio);
		} else {
			float hwRatio = 4.11764705882f;
			rectWidth = (rectHeight * hwRatio);
		}

		GuiRect = new Rect(rectX,rectY,rectWidth,rectHeight);

	}

	/**
	 * Initialize ChatGUI
	 */
	public void OnGUI()
	{
		if (!this.IsVisible)
		{
			return;
		}
		
		GUI.skin.label.wordWrap = true;
		// GUI.skin.button.richText = true;      
		// this allows toolbar buttons to have bold/colored text. nice to indicate new msgs
		// GUILayout.Button("<b>lala</b>");      
		// as richText, html tags could be in text
		
		
		if (Event.current.type == EventType.KeyDown && 
		    (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
		{
			if ("ChatInput".Equals(GUI.GetNameOfFocusedControl()))
			{
				// focus on input -> submit it
				GuiSendsMsg();
				return; // showing the now modified list would result in an error. to avoid this, we just skip this single frame
			}
			else
			{
				// assign focus to input
				GUI.FocusControl("ChatInput");
			}
		}
		
		GUI.SetNextControlName("");
		GUILayout.BeginArea(this.GuiRect);
		
		GUILayout.FlexibleSpace();
		
		if (this.chatClient == null || this.chatClient.State != ChatState.ConnectedToFrontEnd)
		{
			GUILayout.Label("Not in chat yet.");
		}
		else
		{
			List<string> channels = new List<string>(this.chatClient.PublicChannels.Keys);    // this could be cached
			int countOfPublicChannels = channels.Count;
			channels.AddRange(this.chatClient.PrivateChannels.Keys);
			
			if (channels.Count > 0)
			{
				int previouslySelectedChannelIndex = this.selectedChannelIndex;
				int channelIndex = channels.IndexOf(this.selectedChannelName);
				this.selectedChannelIndex = (channelIndex >= 0) ? channelIndex : 0;
				
				this.selectedChannelIndex = GUILayout.Toolbar(
					this.selectedChannelIndex, channels.ToArray(), GUILayout.ExpandWidth(false));
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
				
				this.doingPrivateChat = (this.selectedChannelIndex >= countOfPublicChannels);
				this.selectedChannelName = channels[this.selectedChannelIndex];
				
				if (this.selectedChannelIndex != previouslySelectedChannelIndex)
				{
					// changed channel -> scroll down, if private: pre-fill "to" field with target user's name
					this.scrollPos.y = float.MaxValue;
					if (this.doingPrivateChat)
					{
						string[] pieces = this.selectedChannelName.Split(new char[] {':'}, 3);
						this.userIdInput = pieces[1];
					}
				}
				
				GUILayout.Label(ChatTest.WelcomeText);
				
				if (this.chatClient.TryGetChannel(selectedChannelName, this.doingPrivateChat, out this.selectedChannel))
				{
					for (int i = 0; i < this.selectedChannel.Messages.Count; i++)
					{
						string sender = this.selectedChannel.Senders[i];
						object message = this.selectedChannel.Messages[i];
						GUILayout.Label(string.Format("{0}: {1}", sender, message));
					}
				}
				
				GUILayout.EndScrollView();
			}
		}
		
		
		GUILayout.BeginHorizontal();
		if (doingPrivateChat)
		{
			GUILayout.Label("to:", GUILayout.ExpandWidth(false));
			GUI.SetNextControlName("WhisperTo");
			this.userIdInput = GUILayout.TextField(
				this.userIdInput, GUILayout.MinWidth(100), GUILayout.ExpandWidth(false));
			string focussed = GUI.GetNameOfFocusedControl();
			if (focussed.Equals("WhisperTo"))
			{
				if (this.userIdInput.Equals("username"))
				{
					this.userIdInput = "";
				}
			}
			else if (string.IsNullOrEmpty(this.userIdInput))
			{
				this.userIdInput = "username";
			}
			
		}
		GUI.SetNextControlName("ChatInput");
		inputLine = GUILayout.TextField(inputLine);
		if (GUILayout.Button("Send", GUILayout.ExpandWidth(false)))
		{
			GuiSendsMsg();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	/**
	 * Handle Message sending on GUI
	 */
	private void GuiSendsMsg()
	{
		if (string.IsNullOrEmpty(this.inputLine))
		{
			
			GUI.FocusControl("");
			return;
		}
		
		if (this.inputLine[0].Equals('\\'))
		{
			string[] tokens = this.inputLine.Split(new char[] {' '}, 2);
			if (tokens[0].Equals("\\help"))
			{
				this.PostHelpToCurrentChannel();
			}
			if (tokens[0].Equals("\\state"))
			{
				int newState = int.Parse(tokens[1]);
				this.chatClient.SetOnlineStatus(newState, new string[] {
					"i am state " + newState 
				});  // this is how you set your own state and (any) message
			}
			else if (tokens[0].Equals("\\subscribe") && !string.IsNullOrEmpty(tokens[1]))
			{
				this.chatClient.Subscribe(tokens[1].Split(new char[] {' ', ','}));
			}
			else if (tokens[0].Equals("\\unsubscribe") && !string.IsNullOrEmpty(tokens[1]))
			{
				this.chatClient.Unsubscribe(tokens[1].Split(new char[] {' ', ','}));
			}
			else if (tokens[0].Equals("\\clear"))
			{
				if (this.doingPrivateChat)
				{
					this.chatClient.PrivateChannels.Remove(this.selectedChannelName);
				}
				else
				{
					ChatChannel channel;
					if (this.chatClient.TryGetChannel(this.selectedChannelName, this.doingPrivateChat, out channel))
					{
						channel.ClearMessages();
					}
				}
			}
			else if (tokens[0].Equals("\\msg") && !string.IsNullOrEmpty(tokens[1]))
			{
				string[] subtokens = tokens[1].Split(new char[] {' ', ','}, 2);
				string targetUser = subtokens[0];
				string message = subtokens[1];
				this.chatClient.SendPrivateMessage(targetUser, message);
			}
			else if (tokens[0].Equals("\\allsmall") && !string.IsNullOrEmpty(tokens[1]))
			{
				string input = tokens[1];
				AllChat(false, input);
			}
			else if (tokens[0].Equals("\\allbig") && !string.IsNullOrEmpty(tokens[1]))
			{
				string input = tokens[1];
				AllChat(true, input);
			}
		}
		else
		{
			if (this.doingPrivateChat)
			{
				this.chatClient.SendPrivateMessage(this.userIdInput, this.inputLine);
			}
			else
			{
				this.chatClient.PublishMessage(this.selectedChannelName, this.inputLine);
			}
		}
		
		this.inputLine = "";
		GUI.FocusControl("");
	}

	/**
	 * Overload for intializing this interactive object. 
	 * 
	 * Arguments
	 * - boolean big - True: Big message display, Small: In chat box display
	 * - string input - message
	 */
	public void AllChat(bool big, string input) {
		if (big) {
			Big(input);
		} else {
			Small(input);
		}
	}

	/**
	 * Handle in chat box all chat message
	 * 
	 * Arguments
	 * - string input - message
	 */
	[PunRPC]
	void Small(string input) {
		this.selectedChannel.Add("GAME", input);
	}

	/**
	 * Handle all chat message display
	 * 
	 * Arguments
	 * - string input - message
	 */
	[PunRPC]
	public void Big(string input) {
		GetComponent<AudioSource>().Play();
		GameObject bigText = Instantiate(Resources.Load("BigText")) as GameObject;
		GameObject UI = GameObject.Find("Main_Canvas");
		bigText.transform.SetParent(UI.transform, false);
		bigText.GetComponent<Text>().text = input;
	}

	private void PostHelpToCurrentChannel()
	{
		ChatChannel channelForHelp = this.selectedChannel;
		if (channelForHelp != null)
		{
			channelForHelp.Add("info", ChatTest.HelpText);
		}
		else
		{
			Debug.LogError("no channel for help");
		}
	}

	/**
	 * Handle channels initialization
	 */
	public void OnConnected()
	{
		if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
		{
			this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
		}
		
		this.chatClient.AddFriends(new string[] {"tobi", "ilya"});          
		// Add some users to the server-list to get their status updates
		this.chatClient.SetOnlineStatus(ChatUserStatus.Online);             
		// You can set your online state (without a mesage).
	}

	/**
	 * Debug message
	 * 
	 * Arguments
	 * - DebugLevel level - Debug level
	 * - string message - debug message
	 */
	public void DebugReturn(DebugLevel level, string message)
	{
		//		Debug.Log(message);
	}

	/**
	 * Handle chat disconnect
	 */
	public void OnDisconnected()
	{
	}

	/**
	 * Subscribe to channels
	 * 
	 * Arguments
	 * - string[] channels - list of channels
	 * - bool[] results - list of channel status
	 */
	public void OnSubscribed(string[] channels, bool[] results)
	{
		
		// demo can automatically send a "hi" to subscribed channels. in a game you usually only send user's input!
		if (this.DemoPublishOnSubscribe)
		{
			foreach (string channel in channels)
			{
				this.chatClient.PublishMessage(channel, "says 'hi' in OnSubscribed(). "); 
				// you don't HAVE to send a msg on join but you could.
			}
		}
	}

	/**
	 * Unsubscribe to channels
	 * 
	 * Arguments
	 * - string channel - channel
	 */
	public void OnUnsubscribed(string[] channels)
	{
	}

	/**
	 * Retrieve message
	 * 
	 * Arguments
	 * - string channelName - channel
	 * - string[] senders - list of senders
	 * - string[] messages - list of messages
	 */
	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		GetComponent<AudioSource>().Play();
		if (channelName.Equals(this.selectedChannelName))
		{
			this.scrollPos.y = float.MaxValue;
		}
	}

	/**
	 * Handle sending of private message
	 * 
	 * Arguments
	 * - string sender - sender
	 * - object message - message object
	 * - string channelName - channel
	 */
	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		// as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
		// you also get messages that you sent yourself. 
		// in that case, the channelName is determinded by the target of your msg
	}

	/**
	 * Handle status updates
	 * 
	 * Arguments
	 * - string user - user
	 * - int status - status
	 * - bool gotMessage - if message is retrieved
	 * - object message - message object
	 */
	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		// this is how you get status updates of friends.
		// this demo simply adds status updates to the currently shown chat.
		// you could buffer them or use them any other way, too.
		
		ChatChannel activeChannel = this.selectedChannel;
		if (activeChannel != null)
		{
			activeChannel.Add("info", string.Format("{0} is {1}. Msg:{2}", user, status, message));
		}
		
		Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));
	}

	/**
	 * Change chat state
	 * 
	 * Arguments
	 * - ChatState state - state
	 */
	public void OnChatStateChange(ChatState state)
	{
		// use OnConnected() and OnDisconnected()
		// this method might become more useful in the future, when more complex states are being used.
	}
}
