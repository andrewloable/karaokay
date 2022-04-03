using KaraokayListMaker.models;
using Newtonsoft.Json;

namespace KaraokayListMaker
{
  public partial class Form1 : Form
  {
    private List<KListItem> List;
    public Form1()
    {
      InitializeComponent();
    }

    private void webView21_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
    {
      textBox1.Text = webView21.Source.ToString();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
      var temp = webView21.Source.ToString().Split("?");
      if (temp.Length == 2)
      {
        var temp2 = temp[1].Split("=");
        var id = temp2[1];
        var client = new HttpClient();
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key=AIzaSyDCubKlP2oFJ0rgjbqbeodiUJoYEuPtpJE");
        request.Method = HttpMethod.Get;
        request.Headers.Add("Accept", "application/json");

        var response = await client.SendAsync(request);
        var result = await response.Content.ReadAsStringAsync();
        var jobj = JsonConvert.DeserializeObject<YtResult>(result);
        if (this.List == null)
        {
          this.List = new List<KListItem>();
        }
        if (jobj?.Items != null && jobj.Items.Length > 0)
        {
          var check = this.List.FirstOrDefault(r => r.Id == jobj.Items[0].Id);
          if (check != null)
          {
            MessageBox.Show("Video Already In The List");
          }
          else
          {
            this.List.Add(new KListItem
            {
              Id = jobj.Items[0].Id,
              Title = jobj.Items[0].Snippet.Title
                .ToUpper()
                .Replace("| KARAOKE VERSION | KARAFUN", string.Empty)
                .Replace("(KARAOKE VERSION)", string.Empty)
                .Replace("(HD KARAOKE)", string.Empty)
                .Replace("(KARAOKE)", string.Empty)
                .Trim(),
              Thumbnail = jobj.Items[0].Snippet.Thumbnails.Default.Url,
              Duration = ytDurationToSeconds(jobj.Items[0].ContentDetail.Duration)
            });
            LoadList();
          }
        }
      }
      else
      {
        MessageBox.Show("invalid video");
      }
    }

    private void LoadList()
    {
      listBox1.Items.Clear();
      listBox1.Items.AddRange(List.ToArray());
      label1.Text = $"Total Count: {List.Count}";
    }

    private void button3_Click(object sender, EventArgs e)
    {
      var res = new OpenFileDialog();
      res.Title = "Open Karaokay Song List";
      res.Filter = "Karaokay Files | *.karaokay";
      res.DefaultExt = "karaokay";
      res.Multiselect = false;
      if (res.ShowDialog() == DialogResult.OK)
      {
        if (this.List == null)
        {
          this.List = new List<KListItem>();
        }
        var content = File.ReadAllText(res.FileName);
        if (content != null && content != String.Empty)
        {
          var temp = JsonConvert.DeserializeObject<List<KListItem>>(content);
          foreach (var o in temp)
          {
            var check = this.List.FirstOrDefault(r => r.Id == o.Id);
            if (check == null)
            {
              this.List.Add(o);
            }
          }
          LoadList();
        }
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      var res = new SaveFileDialog();
      res.Title = "Save Karaokay Song List";
      res.Filter = "Karaokay Files | *.karaokay";
      res.DefaultExt = "karaokay";
      if (res.ShowDialog() == DialogResult.OK)
      {
        var content = JsonConvert.SerializeObject(this.List.ToArray(), Formatting.None);
        File.WriteAllText(res.FileName, content);
      }
    }

    private void removeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (listBox1.SelectedItems.Count > 0)
      {
        var toremove = listBox1.SelectedItems;
        foreach (var o in toremove)
        {
          this.List.Remove((KListItem)o);
        }
        LoadList();
      }
    }

    private int ytDurationToSeconds(string duration)
    {
      var hours = 0;
      var minutes = 0;
      var seconds = 0;

      duration = duration.Replace("PT", "");
      if (duration.IndexOf("H") > -1)
      {
        var split = duration.Split("H");
        hours = int.Parse(split[0]);
        duration = split[1];
      }
      if (duration.IndexOf("M") > -1)
      {
        var split = duration.Split("M");
        minutes = int.Parse(split[0]);
        duration = split[1];
      }
      if (duration.IndexOf("S") > -1)
      {
        var split = duration.Split("S");
        seconds = int.Parse(split[0]);
      }
      return (hours * 60 * 60) + (minutes * 60) + seconds;
    }

    private async Task<int> getDuration(string id)
    {
      var client = new HttpClient();
      var request = new HttpRequestMessage();
      request.RequestUri = new Uri($"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={id}&key=AIzaSyDCubKlP2oFJ0rgjbqbeodiUJoYEuPtpJE&part=contentDetails");
      request.Method = HttpMethod.Get;
      request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");
      request.Headers.Add("Accept", "application/json");
      request.Headers.Add("Referer", "karaokay.loable.tech");

      var response = await client.SendAsync(request);
      var result = await response.Content.ReadAsStringAsync();
      var yt = JsonConvert.DeserializeObject<YtResult>(result);
      return ytDurationToSeconds(yt.Items[0].ContentDetail.Duration);
    }

    private async void button4_Click(object sender, EventArgs e)
    {
      foreach (var o in this.List)
      {
        o.Duration = await getDuration(o.Id);
      }
    }
  }
}
