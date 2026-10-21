export const environment = {
  production: true,
  endpoints: {
    apiBaseURL: 'https://localhost:7269/',
    login: 'api/v1.0/lms/user/login',
    addUser: 'api/v1.0/lms/user/register',
    getCourse: 'api/v1.0/lms/courses/getall',
    addCourse: 'api/v1.0/lms/courses/add',
    deleteCourse: 'api/v1.0/lms/courses/delete/{coursename}',
    getCourseByTechnology: 'api/v1.0/lms/courses/info/{technology}',
    getCourseByDuration: 'api/v1.0/lms/courses/get/{technology}/{durationFromRange}/{durationToRange}'
  }
};
