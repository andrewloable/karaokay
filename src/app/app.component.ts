import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import {Song} from '../app/models/song.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  
  currentSongId = '';
  songs: Song[] = [];
  filterdSongs: Song[] = [];
  searchText = '';
  queueSongs: Song[] = [];
  isPaused = false;
  timeout!: number;

  /**
   *
   */
  constructor(
    private http: HttpClient,
  ) { }

  ngOnInit(): void {
    void this.loadSongLibrary();
  }
  
  async loadSongLibrary(): Promise<void> {
    const res = await firstValueFrom(this.http.get<Song[]>('assets/data.json'));
    this.songs = res.sort( (a,b) => {
      if (a.title > b.title) return 1;
      if (a.title < b.title) return -1;
      return 0;
    });
    this.filterdSongs = this.songs.slice(0, 25);
    this.getNextSong();
  }

  filterSearch(): void {
    if (this.searchText.length > 2) {
      this.filterdSongs = this.songs.filter(r => r.title.includes(this.searchText.toUpperCase()));
    }
    else {
      this.filterdSongs = this.songs.slice(0, 25);
    }
  }

  queueSong(s: Song): void {
    this.queueSongs.unshift(s);
  }

  getNextSong(): void {
    if (this.queueSongs.length > 0) {
      const newsong = this.queueSongs.shift();
      this.currentSongId = newsong?.id ?? '';
    }
    else {
      const newsong = this.songs[Math.floor(Math.random() * this.songs.length)];
      this.currentSongId = newsong?.id ?? '';
    }
    console.log(`playing ${this.currentSongId}`);
    void this.otherWindowPlay();
  }

  async otherWindowPlay(): Promise<void> {
    const duration = await this.getYtDuration(this.currentSongId);
    const wind = window.open(`https://www.youtube.com/v/${this.currentSongId}?autoplay=1&fs=1`, '_blank', `location=no, toolbar=no, status=no, menubar=no, titlebar=no, width=${screen.availWidth}, height=${screen.availHeight}`);
    if ((wind?.document.fullscreenElement !== undefined && document.fullscreenElement === null)){
      wind.document.body.requestFullscreen();
    }
    //wind?.resizeTo(screen.width, screen.height);
    window.setTimeout( () => {
      wind?.close();
      this.getNextSong();
    }, duration * 1000);
  }

  async getYtDuration(id: string): Promise<number> {
    const headersList = {
      "Accept": "application/json"
     }
     
     const resp = await fetch(`https://www.googleapis.com/youtube/v3/videos?part=snippet&id=${id}&key=AIzaSyDCubKlP2oFJ0rgjbqbeodiUJoYEuPtpJE&part=contentDetails`, { 
       method: "GET",
       headers: headersList
     });
     const dt = await resp.json();
     return this.ytDurationToSeconds(dt.items[0].contentDetails.duration);
  }

  ytDurationToSeconds(duration: string): number {
    let hours   = 0;
    let minutes = 0;
    let seconds = 0;
  
    // Remove PT from string ref: https://developers.google.com/youtube/v3/docs/videos#contentDetails.duration
    duration = duration.replace('PT','');
  
    // If the string contains hours parse it and remove it from our duration string
    if (duration.indexOf('H') > -1) {
      const hours_split = duration.split('H');
      hours       = parseInt(hours_split[0]);
      duration    = hours_split[1];
    }
  
    // If the string contains minutes parse it and remove it from our duration string
    if (duration.indexOf('M') > -1) {
      const minutes_split = duration.split('M');
      minutes       = parseInt(minutes_split[0]);
      duration      = minutes_split[1];
    }
  
    // If the string contains seconds parse it and remove it from our duration string
    if (duration.indexOf('S') > -1) {
      const seconds_split = duration.split('S');
      seconds       = parseInt(seconds_split[0]);
    }
  
    // Math the values to return seconds
    return (hours * 60 * 60) + (minutes * 60) + seconds;
  }
}
