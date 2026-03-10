using System;
using System.Collections.Generic;
using UnityEngine;

public class TabControl : MonoBehaviour
{
	public List<TabItem> TabItems;
	private TabItem _current;

	private void Awake()
	{
		foreach(var tabItem in TabItems)
		{
			var capturedTab = tabItem;

			tabItem.TabButton.onClick.AddListener(
				() => TabClicked(capturedTab));
		}
	}

	private void Start()
	{
		if (TabItems.Count > 0)
			TabClicked(TabItems[0]);
	}

	private void TabClicked(TabItem selectedTab)
	{
		if (_current == selectedTab)
		{
			_current = null;
		}
		else
		{
			_current = selectedTab;
		}

		foreach (var tabItem in TabItems)
		{
			var selected = tabItem == _current;
			tabItem.SetSelected(selected);
		}
	}
}
