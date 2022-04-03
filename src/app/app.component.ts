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
  
  currentSong!: Song;
  songs: Song[] = [];
  filterdSongs: Song[] = [];
  searchText = '';
  queueSongs: Song[] = [];
  isPlaying = false;
  timeout!: number;
  isAutoNext = true;
  currentWindow!: Window | null;

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
    this.filterdSongs = this.songs;
  }

  filterSearch(): void {
    if (this.searchText.length > 2) {
      this.filterdSongs = this.songs.filter(r => r.title.includes(this.searchText.toUpperCase()));
    }
    else {
      this.filterdSongs = this.songs;
    }
  }

  queueSong(s: Song): void {
    this.queueSongs.unshift(s);
  }

  getNextSong(): void {
    if (this.queueSongs.length > 0) {
      const val = this.queueSongs.shift();
      if (val) this.currentSong = val;
    }
    else {
      const val = this.songs[Math.floor(Math.random() * this.songs.length)];
      if (val) this.currentSong = val;
    }
    console.log(`playing ${this.currentSong.id}`);
    void this.otherWindowPlay();
  }

  async otherWindowPlay(): Promise<void> {
    if (this.currentWindow) {
      this.isAutoNext = false;
      this.currentWindow.close();
    }

    this.currentWindow = window.open(`https://www.youtube.com/v/${this.currentSong.id}?autoplay=1&fs=1`, '_blank', `location=no, toolbar=no, status=no, menubar=no, titlebar=no, width=${screen.availWidth}, height=${screen.availHeight}`);
    this.isPlaying = true;
    this.currentWindow?.resizeTo(screen.width, screen.height);
    const polltimer = window.setInterval( () => {
      if (this.currentWindow?.closed) {
        window.clearInterval(polltimer);
        clearTimeout(this.timeout);
        if (this.isAutoNext) {
          this.getNextSong();
        }
      }
    }, 1000);
    clearTimeout(this.timeout);
    this.timeout = window.setTimeout( () => {
      this.currentWindow?.close();
      this.getNextSong();
    }, this.currentSong.duration * 1000);
    this.isAutoNext = true;
  }
}
