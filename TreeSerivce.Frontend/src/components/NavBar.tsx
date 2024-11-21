import React, { useState } from "react";

interface NavBarProps {
	onGetNode: (id: string) => void;
	onGetChildren: (id: string) => void;
}

export const NavBar: React.FC<NavBarProps> = ({ onGetNode, onGetChildren }) => {
	const [modalOpen, setModalOpen] = useState(false);
	const [guid, setGuid] = useState<string>("");
	const [actionType, setActionType] = useState<
		"getNode" | "getChildren" | null
	>(null);

	const handleAction = () => {
		if (actionType === "getNode") {
			onGetNode(guid);
		} else if (actionType === "getChildren") {
			onGetChildren(guid);
		}
		setGuid("");
		setModalOpen(false);
	};

	return (
		<>
			{/* NavBar */}
			<div className="flex justify-between items-center px-4 py-2 bg-gray-800 text-white">
				<h1 className="text-lg font-semibold">Tree Service</h1>
				<div className="flex gap-4">
					<button
						className="px-4 py-2 bg-blue-500 rounded hover:bg-blue-600"
						onClick={() => {
							setActionType("getNode");
							setModalOpen(true);
						}}
					>
						Get Node
					</button>
					<button
						className="px-4 py-2 bg-green-500 rounded hover:bg-green-600"
						onClick={() => {
							setActionType("getChildren");
							setModalOpen(true);
						}}
					>
						Get All Nodes
					</button>
				</div>
			</div>

			{/* Modal */}
			{modalOpen && (
				<div className="fixed inset-0 flex items-center justify-center bg-gray-800 bg-opacity-75">
					<div className="bg-white rounded-lg shadow-lg p-6 w-96">
						<h2 className="text-lg font-semibold mb-4">
							{actionType === "getNode"
								? "Enter GUID for Node"
								: "Enter GUID for Children"}
						</h2>
						<input
							type="text"
							className="w-full px-4 py-2 border rounded focus:outline-none"
							placeholder="Enter GUID"
							value={guid}
							onChange={(e) => setGuid(e.target.value)}
						/>
						<div className="flex justify-end gap-4 mt-4">
							<button
								className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600"
								onClick={() => setModalOpen(false)}
							>
								Cancel
							</button>
							<button
								className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
								onClick={handleAction}
								disabled={!guid}
							>
								Submit
							</button>
						</div>
					</div>
				</div>
			)}
		</>
	);
};
