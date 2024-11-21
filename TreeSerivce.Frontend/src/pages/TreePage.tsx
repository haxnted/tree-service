import React, { useState } from "react";
import { NavBar } from "../components/NavBar";
import { NodeTree } from "../components/NodeTree";

export const TreePage: React.FC = () => {
	const [nodes, setNodes] = useState<NodeTree[]>([]);
	const [isSingleNode, setIsSingleNode] = useState(false);

	const fetchNodeById = async (id: string) => {
		try {
			const response = await fetch(`http://localhost:5291/api/Tree/${id}`);
			if (response.ok) {
				const data = await response.json();
				setNodes([data.result]);
				setIsSingleNode(true);
			} else {
				alert("Failed to fetch node. Check the GUID and try again.");
			}
		} catch (error) {
			console.error("Error fetching node:", error);
		}
	};

	const fetchChildrenById = async (id: string) => {
		try {
			const response = await fetch(
				`http://localhost:5291/api/Tree/${id}/childrens`
			);
			if (response.ok) {
				const data = await response.json();
				setNodes(data.result.childrenList || []);
				setIsSingleNode(false);
			} else {
				alert("Failed to fetch nodes. Check the GUID and try again.");
			}
		} catch (error) {
			console.error("Error fetching children nodes:", error);
		}
	};

	return (
		<div className="min-h-screen bg-gray-100">
			<NavBar onGetNode={fetchNodeById} onGetChildren={fetchChildrenById} />
			<div className="p-4">
				{nodes.length > 0 ? (
					isSingleNode ? (
						// Отображение одной ноды
						<NodeTree
							node={nodes[0]}
							onUpdate={(node) => console.log("Update Node:", node)}
							onDelete={(id) => console.log("Delete Node:", id)}
						/>
					) : (
						// Отображение всех нод
						nodes.map((node) => (
							<NodeTree
								key={node.id}
								node={node}
								onUpdate={(node) => console.log("Update Node:", node)}
								onDelete={(id) => console.log("Delete Node:", id)}
							/>
						))
					)
				) : (
					<p className="text-center text-gray-500">
						No nodes to display. Use the navigation bar to fetch nodes.
					</p>
				)}
			</div>
		</div>
	);
};
