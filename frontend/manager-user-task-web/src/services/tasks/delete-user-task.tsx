"use server";

import { cookies } from "next/headers";

const urlListRealms = `${process.env.API_URL}/UserTask`

export default async function DeleteUserTask(id: any): Promise<any | null> {

  const bearerToken = cookies().get("token-jwt")?.value;

  const url = `${urlListRealms}/${id}`;

  const response = await fetch(
    url,
    {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${bearerToken}`
      },
      next: {
        tags: ["delete-user-task"]
      }
    }
  );

  if (!response?.ok) return false;

  return await true;
}
