import { Component, OnInit } from '@angular/core';
import { Booking } from 'src/app/admin-dashboard/models/Booking.model';
import { Router } from '@angular/router';
import { BookingsService } from '../../services/bookings-service';
import { MyBookingDto } from '../../models/mybooking.dto';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-booings',
  templateUrl: './user-bookings.component.html',
  styleUrls: ['./user-bookings.component.css'],
})
export class UserBookingsComponent implements OnInit {
  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private bookingsService: BookingsService
  ) {}
  myBookings!: MyBookingDto[];

  ngOnInit(): void {
    this.loadUserBookings();
  }

  loadUserBookings(): void {
    this.bookingsService.getUserBookings().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.myBookings = res.data;
        } else {
          this.snackBar.open(
            res.message || 'Failed to load bookings',
            'Close',
            {
              duration: 3000,
              panelClass: ['error-snackbar'],
            }
          );
        }
      },
      error: () => {
        this.snackBar.open('Failed to load bookings', 'Close', {
          duration: 3000,
          panelClass: ['error-snackbar'],
        });
      },
    });
  }

  canDeleteBooking(booking: MyBookingDto): boolean {
    const now = new Date();
    const bookingStart = this.getBookingStartDateTime(booking);
    const diffInMinutes =
      (bookingStart.getTime() - now.getTime()) / (1000 * 60);
    return diffInMinutes > 30;
  }

  onDeleteClicked(booking: MyBookingDto): void {
    if (this.canDeleteBooking(booking)) {
      this.snackBar
        .open('Click again to confirm deletion', 'Delete', {
          duration: 7000,
          panelClass: ['confirm-snackbar'],
        })
        .onAction()
        .subscribe(() => this.deleteBooking(booking.id));
    } else {
      this.snackBar.open(
        'You can’t delete a booking less than 30 minutes before start time.',
        'Close',
        {
          duration: 3000,
          panelClass: ['error-snackbar'],
        }
      );
    }
  }

  private deleteBooking(bookingId: string): void {
    this.bookingsService.deleteBooking(bookingId).subscribe({
      next: (res) => {
        if (res.success) {
          this.myBookings = this.myBookings.filter(
            (booking) => booking.id !== bookingId
          );
          this.snackBar.open('Booking deleted successfully', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar'],
          });
        } else {
          this.snackBar.open(
            res.message || 'Failed to delete booking',
            'Close',
            {
              duration: 3000,
              panelClass: ['error-snackbar'],
            }
          );
        }
      },
      error: () => {
        this.snackBar.open('Failed to delete booking', 'Close', {
          duration: 3000,
          panelClass: ['error-snackbar'],
        });
      },
    });
  }

  private getBookingStartDateTime(booking: MyBookingDto): Date {
    const [hour, minute] = booking.startTime.split(':').map(Number);
    const bookingStart = new Date(booking.date);
    bookingStart.setHours(hour, minute, 0, 0);
    return bookingStart;
  }
}
