"use server";

import { cookies } from "next/headers";

const urlListRealms = `${process.env.API_URL}/UserTask/list-user-task-grouped`

export default async function ListUserTask(currentPage: number, pageSize: number): Promise<any | null> {

  const bearerToken = cookies().get("token-jwt")?.value;

  const url = `${urlListRealms}?Page=${currentPage}&PageSize=${pageSize}`;

  const response = await fetch(
    url,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${bearerToken}`
      },
      next: {
        tags: ["user-task-group"]
      }
    }
  );

  if (!response?.ok) return null;

  return await response.json();
}
