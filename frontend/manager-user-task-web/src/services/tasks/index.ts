
import CreateUserTask from "@/services/tasks/create-user-task"
import DeleteUserTask from "@/services/tasks/delete-user-task"
import GetUserTask from "@/services/tasks/get-user-task"
import ListUserTaskGroup from "@/services/tasks/list-users-tasks-group"
import UpdateUserTask from "@/services/tasks/update-user-task"
export const TasksService = {
  List: ListUserTaskGroup,
  New: CreateUserTask,
  Remove: DeleteUserTask,
  Get: GetUserTask,
  Update: UpdateUserTask
}