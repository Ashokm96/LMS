import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  courses = [
    {
      name: 'fgf',
      description: 'Description 1',
      technology: 'Technology 1',
      duration: 60,
      launchUrl: 'https://example.com/course1'
    },
    {
      name: 'ww',
      description: 'Description 2',
      technology: 'Technology 2',
      duration: 50,
      launchUrl: 'https://example.com/course2'
    },
    {
      name: 'rr',
      description: 'Description 2',
      technology: 'Technology 2',
      duration: 150,
      launchUrl: 'https://example.com/course2'
    }
    // Add more courses as needed
  ];

  searchQuery = '';
  filteredCourses: any[] = [];

  ngOnInit(): void {
    // Initialize filteredCourses with all courses
    this.filteredCourses = [...this.courses];
  }

  addCourse() {
    // Implement the logic to add a new course
    // You can open a dialog or navigate to a new page for adding a course
  }

  deleteCourse(course: any) {
    // Implement the logic to delete the selected course
    // You may want to ask for confirmation before deleting
  }

  logout() {
    // Implement the logout functionality
    // You can navigate to the login page or clear user session
  }

  // Implement a filter function for search
  //filterCourses() {
  //  this.filteredCourses = this.courses.filter((course) => {
  //    return (
  //      course.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
  //      course.description.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
  //      course.technology.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
  //      course.duration.toLowerCase().includes(this.searchQuery.toLowerCase())
  //    );
  //  });
  //}
  filterCourses() {
    this.filteredCourses = this.courses.filter((course) => {
      const searchQuery = this.searchQuery.toLowerCase();

      // Convert duration to a string for comparison
      const durationString = course.duration.toString().toLowerCase();

      return (
        course.name.toLowerCase().includes(searchQuery) ||
        course.description.toLowerCase().includes(searchQuery) ||
        course.technology.toLowerCase().includes(searchQuery) ||
        durationString.includes(searchQuery)
      );
    });
  }

}
