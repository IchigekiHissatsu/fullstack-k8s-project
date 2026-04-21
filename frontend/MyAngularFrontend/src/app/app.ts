import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookService } from './services/book.service';
import { Book } from './models/book.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  books: Book[] = [];
  pagedBooks: Book[] = [];
  
  // Lapozáshoz beállítások
  currentPage = 1;
  pageSize = 3; // Egyszerre 3 könyv jelenjen meg a listában

  constructor(private bookService: BookService) {}

  ngOnInit() {
    this.loadBooks();
  }

  // Adatok lekérése a Backend-től a Service-en keresztül
  loadBooks() {
    this.bookService.getBooks().subscribe({
      next: (data) => {
        this.books = data;
        this.updatePage();
      },
      error: (err) => {
        console.error('Hiba a könyvek betöltésekor:', err);
      }
    });
  }

  // Kiszámolja, melyik könyvek jelenjenek meg az aktuális oldalon
  updatePage() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.pagedBooks = this.books.slice(startIndex, startIndex + this.pageSize);
  }

  // Következő oldalra lépés
  nextPage() {
    if ((this.currentPage * this.pageSize) < this.books.length) {
      this.currentPage++;
      this.updatePage();
    }
  }

  // Előző oldalra lépés
  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePage();
    }
  }
}